import { Button, Column, Host, ListItem, RNHostView, Row, Spacer, Text } from "@expo/ui";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { Image } from "expo-image";
import { type Href, Link } from "expo-router";
import * as Linking from "expo-linking";
import * as WebBrowser from "expo-web-browser";
import type { ReactNode } from "react";
import { useState } from "react";

import { playApiUrl } from "@/api/play-api-client";
import { getAuthMeQueryKey, useGetAuthMe } from "@/gen/play-api/hooks/AuthHooks/useGetAuthMe";
import { usePostAuthLogout } from "@/gen/play-api/hooks/AuthHooks/usePostAuthLogout";
import { usePostAuthMobileExchange } from "@/gen/play-api/hooks/AuthHooks/usePostAuthMobileExchange";
import { useGetApiMeLists } from "@/gen/play-api/hooks/ListsHooks/useGetApiMeLists";
import { useAppTheme } from "@/hooks/use-app-theme";
import { posthog } from "@/lib/posthog";

import { authStorage } from "./auth-storage";
import { sessionTokenQueryKey } from "./auth-query";
import { ProfileActionButton } from "./profile-action-button";

export function ProfileScreen() {
  const colors = useAppTheme();
  const queryClient = useQueryClient();
  const [message, setMessage] = useState<string | null>(null);
  const session = useQuery({
    queryKey: sessionTokenQueryKey,
    queryFn: authStorage.get,
    staleTime: Infinity,
  });
  const token = session.data;
  const profile = useGetAuthMe({
    query: {
      enabled: typeof token === "string",
      retry: false,
    },
  });
  const exchange = usePostAuthMobileExchange();
  const logout = usePostAuthLogout();
  const lists = useGetApiMeLists({ query: { enabled: typeof token === "string", retry: false } });

  async function signIn() {
    setMessage(null);

    try {
      const redirectUrl = Linking.createURL("auth/callback", { scheme: "gameindex" });
      const result = await WebBrowser.openAuthSessionAsync(
        `${playApiUrl}/auth/steam/login?returnUrl=${encodeURIComponent(redirectUrl)}`,
        redirectUrl,
      );

      if (result.type !== "success") {
        return;
      }

      const callbackUrl = new URL(result.url);
      const code = callbackUrl.searchParams.get("code");

      if (!code) {
        throw new Error("Steam did not return an exchange code.");
      }

      const session = await exchange.mutateAsync({
        body: { code, state: null },
      });

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

      await authStorage.set(session.accessToken);
      queryClient.setQueryData(sessionTokenQueryKey, session.accessToken);
      await queryClient.invalidateQueries({ queryKey: getAuthMeQueryKey() });
    } catch (error) {
      console.error("Steam sign-in failed.", error);
      setMessage("Steam sign-in failed. Try again.");
    }
  }

  async function handleSignOut() {
    setMessage(null);

    try {
      await logout.mutateAsync(undefined);
      posthog?.capture("account_signed_out", {
        authentication_provider: "steam",
      });
      posthog?.reset();
      await authStorage.clear();
      queryClient.setQueryData(sessionTokenQueryKey, null);
      queryClient.removeQueries({ queryKey: getAuthMeQueryKey() });
    } catch (error) {
      console.error("Steam sign-out failed.", error);
      setMessage("Could not sign out. Try again.");
    }
  }

  if (session.isPending) {
    return (
      <Screen colors={colors}>
        <Text>Loading profile…</Text>
      </Screen>
    );
  }

  if (session.isError) {
    return (
      <Screen colors={colors}>
        <Text>Steam session unavailable. Sign in again.</Text>
        <SignInButton disabled={exchange.isPending} onPress={signIn} />
      </Screen>
    );
  }

  if (typeof token !== "string") {
    return (
      <Screen colors={colors}>
        <Text>Sign in to keep track of your games.</Text>
        {message ? <Text>{message}</Text> : null}
        <SignInButton disabled={exchange.isPending} onPress={signIn} />
      </Screen>
    );
  }

  if (profile.isPending) {
    return (
      <Screen colors={colors}>
        <Text>Loading profile…</Text>
      </Screen>
    );
  }

  if (profile.isError || !profile.data) {
    return (
      <Screen colors={colors}>
        <Text>Steam session unavailable. Sign in again.</Text>
        {message ? <Text>{message}</Text> : null}
        <SignInButton disabled={exchange.isPending} onPress={signIn} />
      </Screen>
    );
  }

  return (
    <Screen colors={colors}>
      <Row alignment="center" spacing={16}>
        <RNHostView matchContents>
          <Image
            source={profile.data.avatarUrl}
            style={{ width: 72, height: 72, borderRadius: 36, backgroundColor: colors.surfaceHigh }}
            contentFit="cover"
          />
        </RNHostView>
        <Column spacing={4}>
          <Text textStyle={{ fontSize: 22, fontWeight: "700" }}>{profile.data.steamName}</Text>
          <Text>{`Steam user · ${profile.data.role}`}</Text>
        </Column>
      </Row>
      <Column
        spacing={4}
        style={{ borderColor: colors.outline, borderWidth: 1, paddingVertical: 16 }}
      >
        <Text textStyle={{ fontSize: 13, fontWeight: "600" }}>Steam ID</Text>
        <Text>{profile.data.steamId}</Text>
      </Column>
      <Column spacing={10}>
        <Row alignment="center">
          <Text textStyle={{ fontSize: 19, fontWeight: "800" }}>My lists</Text>
          <Spacer flexible />
          <Link href="/profile/lists/new" asChild>
            <Button label="Create" variant="text" />
          </Link>
        </Row>
        {lists.data?.map((list) => {
          const href = { pathname: "/profile/lists/[id]", params: { id: list.id } } satisfies Href;
          return (
            <Link key={list.id} href={href} asChild>
              <ListItem
                supportingText={`${list.itemCount} games · ${list.visibility}`}
                trailing={<Text>›</Text>}
              >
                {list.name}
              </ListItem>
            </Link>
          );
        })}
      </Column>
      {message ? <Text>{message}</Text> : null}
      <ProfileActionButton
        disabled={logout.isPending}
        label="Sign out"
        onPress={handleSignOut}
        variant="outlined"
      />
    </Screen>
  );
}

function SignInButton({ disabled, onPress }: { disabled: boolean; onPress: () => void }) {
  return (
    <ProfileActionButton
      disabled={disabled}
      label="Sign in with Steam"
      onPress={onPress}
      variant="filled"
    />
  );
}

function Screen({
  children,
  colors,
}: {
  children: ReactNode;
  colors: ReturnType<typeof useAppTheme>;
}) {
  return (
    <Host style={{ flex: 1, backgroundColor: colors.background }}>
      <Column spacing={20} style={{ padding: 24 }}>
        <Text textStyle={{ fontSize: 28, fontWeight: "700" }}>Profile</Text>
        {children}
      </Column>
    </Host>
  );
}
