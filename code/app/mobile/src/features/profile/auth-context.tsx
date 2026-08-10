import { useQuery } from "@tanstack/react-query";
import * as Linking from "expo-linking";
import * as WebBrowser from "expo-web-browser";
import { createContext, useContext, type ReactNode } from "react";

import { playApiUrl } from "@/api/play-api-client";
import { getAuthMeQueryKey, useGetAuthMe } from "@/gen/play-api/hooks/AuthHooks/useGetAuthMe";
import { usePostAuthLogout } from "@/gen/play-api/hooks/AuthHooks/usePostAuthLogout";
import { usePostAuthMobileExchange } from "@/gen/play-api/hooks/AuthHooks/usePostAuthMobileExchange";
import { queryClient } from "@/lib/query-client";
import { posthog } from "@/lib/posthog";

import {
  authCodeSchema,
  sessionResponseSchema,
  userResponseSchema,
  type AuthUser,
} from "./auth-schemas";
import { authStorage } from "./auth-storage";

export type AuthStatus = "loading" | "authenticated" | "unauthenticated" | "error";

type AuthContextValue = {
  error: Error | null;
  isAuthenticated: boolean;
  isSigningIn: boolean;
  isSigningOut: boolean;
  status: AuthStatus;
  user: AuthUser | null;
  signInWithSteam: () => Promise<void>;
  signOut: () => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);
const sessionTokenQueryKey = ["play", "access-token"] as const;

export function AuthProvider({ children }: { children: ReactNode }) {
  const tokenQuery = useQuery({
    queryKey: sessionTokenQueryKey,
    queryFn: authStorage.get,
    staleTime: Infinity,
    retry: false,
  });
  const token = tokenQuery.data;
  const exchange = usePostAuthMobileExchange();
  const logout = usePostAuthLogout();
  const profile = useGetAuthMe({
    query: {
      enabled: typeof token === "string",
      retry: false,
      select: (response) => userResponseSchema.parse(response),
    },
  });

  async function clearLocalSession() {
    try {
      await authStorage.clear();
    } finally {
      queryClient.setQueryData(sessionTokenQueryKey, null);
      queryClient.removeQueries({ queryKey: getAuthMeQueryKey() });
    }
  }

  async function signInWithSteam() {
    const redirectUrl = Linking.createURL("auth/callback", { scheme: "gameindex" });
    const result = await WebBrowser.openAuthSessionAsync(
      `${playApiUrl}/auth/steam/login?returnUrl=${encodeURIComponent(redirectUrl)}`,
      redirectUrl,
    );

    if (result.type !== "success") {
      return;
    }

    const callbackUrl = new URL(result.url);
    const code = authCodeSchema.parse(callbackUrl.searchParams.get("code"));
    const response = await exchange.mutateAsync({
      body: { code, state: null },
    });
    const parsedSession = sessionResponseSchema.safeParse(response);

    if (!parsedSession.success) {
      throw new Error("Steam returned an invalid mobile session.", { cause: parsedSession.error });
    }

    const session = parsedSession.data;
    await authStorage.set(session.accessToken);
    queryClient.setQueryData(sessionTokenQueryKey, session.accessToken);
    queryClient.setQueryData(getAuthMeQueryKey(), session.user);
    posthog?.identify(session.user.id, {
      $set: {
        steam_id: session.user.steamId,
        steam_name: session.user.steamName,
        role: session.user.role,
      },
    });
    posthog?.capture("steam_sign_in_completed", {
      authentication_provider: "steam",
    });
  }

  async function signOut() {
    try {
      if (typeof token === "string") {
        await logout.mutateAsync(undefined);
      }
      posthog?.capture("account_signed_out", {
        authentication_provider: "steam",
      });
    } finally {
      posthog?.reset();
      await clearLocalSession();
    }
  }

  const status: AuthStatus = tokenQuery.isPending
    ? "loading"
    : tokenQuery.isError
      ? "error"
      : token === null
        ? "unauthenticated"
        : profile.isPending
          ? "loading"
          : profile.isError
            ? isUnauthorized(profile.error)
              ? "unauthenticated"
              : "error"
            : profile.data
              ? "authenticated"
              : "error";

  const value: AuthContextValue = {
    error: tokenQuery.isError
      ? toError(tokenQuery.error)
      : profile.isError
        ? toError(profile.error)
        : null,
    isAuthenticated: status === "authenticated",
    isSigningIn: exchange.isPending,
    isSigningOut: logout.isPending,
    signInWithSteam,
    signOut,
    status,
    user: profile.data ?? null,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider.");
  }

  return context;
}

function isUnauthorized(error: unknown): error is { status: number } {
  return typeof error === "object" && error !== null && "status" in error && error.status === 401;
}

function toError(error: unknown) {
  return error instanceof Error ? error : new Error("Authentication failed.", { cause: error });
}
