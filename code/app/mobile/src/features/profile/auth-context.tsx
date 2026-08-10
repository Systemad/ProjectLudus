import { useQueryClient } from "@tanstack/react-query";
import * as Linking from "expo-linking";
import * as WebBrowser from "expo-web-browser";
import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from "react";

import { playApiUrl } from "@/api/play-api-client";
import { getAuthMeQueryKey, useGetAuthMe } from "@/gen/play-api/hooks/AuthHooks/useGetAuthMe";
import { usePostAuthLogout } from "@/gen/play-api/hooks/AuthHooks/usePostAuthLogout";
import { usePostAuthMobileExchange } from "@/gen/play-api/hooks/AuthHooks/usePostAuthMobileExchange";
import { posthog } from "@/lib/posthog";

import {
  authCodeSchema,
  authTokenSchema,
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

export function AuthProvider({ children }: { children: ReactNode }) {
  const queryClient = useQueryClient();
  const [token, setToken] = useState<string | null | undefined>(undefined);
  const [storageError, setStorageError] = useState<Error | null>(null);
  const exchange = usePostAuthMobileExchange();
  const logout = usePostAuthLogout();
  const profile = useGetAuthMe({
    query: {
      enabled: typeof token === "string",
      retry: false,
      select: (response) => userResponseSchema.parse(response),
    },
  });

  useEffect(() => {
    let cancelled = false;

    void authStorage
      .get()
      .then((storedToken) => {
        if (!cancelled) {
          setToken(storedToken);
        }
      })
      .catch((error: unknown) => {
        if (!cancelled) {
          setStorageError(toError(error));
        }
      });

    return () => {
      cancelled = true;
    };
  }, []);

  const clearLocalSession = useCallback(async () => {
    try {
      await authStorage.clear();
    } finally {
      setToken(null);
      queryClient.removeQueries({ queryKey: getAuthMeQueryKey() });
    }
  }, [queryClient]);

  useEffect(() => {
    if (typeof token !== "string" || !profile.isError || !isUnauthorized(profile.error)) {
      return;
    }

    void clearLocalSession().catch((error: unknown) => {
      console.error("Could not clear the expired Steam session.", error);
    });
  }, [clearLocalSession, profile.error, profile.isError, token]);

  const signInWithSteam = useCallback(async () => {
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
    authTokenSchema.parse(session.accessToken);
    await authStorage.set(session.accessToken);
    queryClient.setQueryData(getAuthMeQueryKey(), session.user);
    setStorageError(null);
    setToken(session.accessToken);
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
  }, [exchange, queryClient]);

  const signOut = useCallback(async () => {
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
  }, [clearLocalSession, logout, token]);

  const status: AuthStatus = storageError
    ? "error"
    : token === undefined
      ? "loading"
      : token === null
        ? "unauthenticated"
        : profile.isPending
          ? "loading"
          : profile.isError
            ? "error"
            : "authenticated";

  const value = useMemo<AuthContextValue>(
    () => ({
      error: storageError ?? (profile.isError ? toError(profile.error) : null),
      isAuthenticated: status === "authenticated",
      isSigningIn: exchange.isPending,
      isSigningOut: logout.isPending,
      signInWithSteam,
      signOut,
      status,
      user: profile.data ?? null,
    }),
    [
      exchange.isPending,
      logout.isPending,
      profile.data,
      profile.error,
      profile.isError,
      signInWithSteam,
      signOut,
      status,
      storageError,
    ],
  );

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
