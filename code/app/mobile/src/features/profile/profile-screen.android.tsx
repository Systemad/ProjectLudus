import {
  Button,
  Card,
  Column,
  FilterChip,
  Icon,
  IconButton,
  LazyColumn,
  Row,
  Text,
} from "@expo/ui/jetpack-compose";
import { Host } from "@expo/ui";
import { fillMaxSize, fillMaxWidth } from "@expo/ui/jetpack-compose/modifiers";
import { router } from "expo-router";
import { useState } from "react";

import { useAppTheme } from "@/hooks/use-app-theme";
import { settingsHref } from "@/navigation/routes";

import { useAuth } from "./auth-context";
import { SignedOutProfile } from "./signed-out-profile";
import { useProfileLibrary, type ProfileLibraryTab } from "./use-profile-library";

const tabs: readonly { value: ProfileLibraryTab; label: string }[] = [
  { value: "wishlist", label: "Wishlist" },
  { value: "lists", label: "My lists" },
  { value: "history", label: "Recently viewed" },
];

export function ProfileScreen() {
  const colors = useAppTheme();
  const { isAuthenticated, isSigningIn, isSigningOut, signInWithSteam, signOut, status, user } =
    useAuth();
  const [tab, setTab] = useState<ProfileLibraryTab>("wishlist");
  const [selectedListId, setSelectedListId] = useState<string | null>(null);
  const { availableLists, activeCollection, isLoading, isError } = useProfileLibrary({
    isAuthenticated,
    tab,
    selectedListId,
  });

  const games = activeCollection?.games ?? [];
  const title =
    tab === "history"
      ? "Recently viewed"
      : (activeCollection?.list?.name ?? (tab === "wishlist" ? "Wishlist" : "My lists"));

  if (status === "loading") {
    return <SignedOutProfile colors={colors} loading />;
  }

  if (!isAuthenticated || !user) {
    return (
      <SignedOutProfile
        colors={colors}
        isSigningIn={isSigningIn}
        onSignIn={() => void signInWithSteam()}
      />
    );
  }

  return (
    <Host style={{ backgroundColor: colors.background, flex: 1 }}>
      <LazyColumn
        modifiers={[fillMaxSize()]}
        contentPadding={{ bottom: 32, end: 16, start: 16, top: 16 }}
        verticalArrangement={{ spacedBy: 12 }}
      >
        <Row modifiers={[fillMaxWidth()]} horizontalArrangement="spaceBetween">
          <Column verticalArrangement={{ spacedBy: 2 }}>
            <Text style={{ typography: "headlineSmall" }}>Profile</Text>
            <Text style={{ typography: "bodyMedium" }}>
              {isAuthenticated && user
                ? user.steamName
                : "Your games, lists, and recently viewed titles"}
            </Text>
          </Column>
          <IconButton onClick={() => router.push(settingsHref)}>
            <Icon source={require("@/assets/icons/settings.xml")} contentDescription="Settings" />
          </IconButton>
        </Row>

        <>
          <Row modifiers={[fillMaxWidth()]} horizontalArrangement="spaceBetween">
            <Text style={{ typography: "titleLarge" }}>My library</Text>
            <Button enabled={!isSigningOut} onClick={() => void signOut()}>
              <Text>Sign out</Text>
            </Button>
          </Row>
          <Row horizontalArrangement={{ spacedBy: 8 }}>
            {tabs.map((item) => (
              <FilterChip
                key={item.value}
                selected={tab === item.value}
                onClick={() => setTab(item.value)}
              >
                <FilterChip.Label>
                  <Text>{item.label}</Text>
                </FilterChip.Label>
              </FilterChip>
            ))}
          </Row>

          {tab === "lists" && availableLists.length > 0 ? (
            <Row horizontalArrangement={{ spacedBy: 8 }}>
              {availableLists.map((list) => (
                <FilterChip
                  key={list.id}
                  selected={selectedListId === list.id}
                  onClick={() => setSelectedListId(list.id)}
                >
                  <FilterChip.Label>
                    <Text>{list.name}</Text>
                  </FilterChip.Label>
                </FilterChip>
              ))}
            </Row>
          ) : null}

          <Text style={{ typography: "titleLarge" }}>{title}</Text>
          <Text style={{ typography: "bodyMedium" }}>
            {isLoading
              ? "Loading games…"
              : isError
                ? "This collection could not be loaded."
                : games.length === 0
                  ? "No games here yet."
                  : `${games.length} ${games.length === 1 ? "game" : "games"}`}
          </Text>

          {games.map((game) => (
            <Card key={game.id} modifiers={[fillMaxWidth()]}>
              <Column verticalArrangement={{ spacedBy: 4 }}>
                <Text style={{ typography: "titleMedium" }}>{game.name}</Text>
                <Text style={{ typography: "bodyMedium" }}>{game.gameTypeName ?? "Game"}</Text>
              </Column>
            </Card>
          ))}
        </>
      </LazyColumn>
    </Host>
  );
}
