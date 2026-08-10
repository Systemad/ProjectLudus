import { Button, Column, Host, ListItem, RNHostView, Row, Spacer, Text } from "@expo/ui";
import { Image } from "expo-image";
import { type Href, Link } from "expo-router";
import type { ReactNode } from "react";
import { useState } from "react";

import { useGetApiMeLists } from "@/gen/play-api/hooks/ListsHooks/useGetApiMeLists";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState } from "@/shared/ui/content-state";

import { useAuth } from "./auth-context";
import { ProfileActionButton } from "./profile-action-button";

export function ProfileScreen() {
  const colors = useAppTheme();
  const [message, setMessage] = useState<string | null>(null);
  const { isAuthenticated, isSigningIn, isSigningOut, signInWithSteam, signOut, status, user } =
    useAuth();
  const lists = useGetApiMeLists({ query: { enabled: isAuthenticated, retry: false } });

  async function signIn() {
    setMessage(null);

    try {
      await signInWithSteam();
    } catch (error) {
      console.error("Steam sign-in failed.", error);
      setMessage("Steam sign-in failed. Try again.");
    }
  }

  async function handleSignOut() {
    setMessage(null);

    try {
      await signOut();
    } catch (error) {
      console.error("Steam sign-out failed.", error);
      setMessage("Could not sign out. Try again.");
    }
  }

  if (status === "loading") {
    return <ContentState status="loading" fullScreen loading={{ label: "Loading profile…" }} />;
  }

  if (!isAuthenticated || !user) {
    return (
      <Screen colors={colors}>
        <Text>
          {status === "error"
            ? "Steam session unavailable. Sign in again."
            : "Sign in to keep track of your games."}
        </Text>
        {message ? <Text>{message}</Text> : null}
        <SignInButton disabled={isSigningIn} onPress={signIn} />
      </Screen>
    );
  }

  return (
    <Screen colors={colors}>
      <Row alignment="center" spacing={16}>
        <RNHostView matchContents>
          <Image
            source={user.avatarUrl}
            style={{ width: 72, height: 72, borderRadius: 36, backgroundColor: colors.surfaceHigh }}
            contentFit="cover"
          />
        </RNHostView>
        <Column spacing={4}>
          <Text textStyle={{ fontSize: 22, fontWeight: "700" }}>{user.steamName}</Text>
          <Text>{`Steam user · ${user.role}`}</Text>
        </Column>
      </Row>
      <Column
        spacing={4}
        style={{ borderColor: colors.outline, borderWidth: 1, paddingVertical: 16 }}
      >
        <Text textStyle={{ fontSize: 13, fontWeight: "600" }}>Steam ID</Text>
        <Text>{user.steamId}</Text>
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
        disabled={isSigningOut}
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
