import { useQueries, useQueryClient } from "@tanstack/react-query";
import { Image } from "expo-image";
import { type Href, Link, router } from "expo-router";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { getIgdbImageUrl } from "@/entities/game/game-image";
import { gamesGetHeroQueryOptions } from "@/gen/hooks/GamesHooks/useGamesGetHero";
import {
  getApiMeListsQueryKey,
  useDeleteApiMeListsId,
  useDeleteApiMeListsIdGamesGameid,
  useGetApiMeLists,
  useGetApiMeListsIdGames,
  usePutApiMeListsId,
} from "@/gen/play-api/index";
import { useAppTheme } from "@/hooks/use-app-theme";
import { posthog } from "@/lib/posthog";
import { commonStyles } from "@/shared/ui/common-styles";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";

export function ListDetail({ id }: { id: string }) {
  const colors = useAppTheme();
  const queryClient = useQueryClient();
  const lists = useGetApiMeLists();
  const games = useGetApiMeListsIdGames({ path: { id }, query: { Page: 1, PageSize: 50 } });
  const remove = useDeleteApiMeListsId();
  const removeGame = useDeleteApiMeListsIdGamesGameid();
  const update = usePutApiMeListsId();
  const list = lists.data?.find((item) => item.id === id);
  const gameItems = games.data?.games ?? [];
  const gameDetails = useQueries({
    queries: gameItems.map((game) => gamesGetHeroQueryOptions({ path: { gameId: game.gameId } })),
  });

  const deleteList = async () => {
    await remove.mutateAsync({ path: { id } });
    posthog?.capture("game_list_deleted", {
      list_id: id,
      visibility: list?.visibility.toLowerCase() ?? null,
    });
    await queryClient.invalidateQueries({ queryKey: getApiMeListsQueryKey() });
    router.back();
  };

  const deleteGame = async (gameId: string) => {
    await removeGame.mutateAsync({ path: { id, gameId } });
    posthog?.capture("game_removed_from_list", { game_id: gameId, list_id: id });
    await queryClient.invalidateQueries({ queryKey: games.queryKey });
    await queryClient.invalidateQueries({ queryKey: getApiMeListsQueryKey() });
  };

  const status = getContentStateStatus(
    lists.isPending || games.isPending,
    lists.isError || games.isError,
    !list,
  );

  if (!list || status !== "ready") {
    return (
      <ContentState
        status={status === "ready" ? "empty" : status}
        fullScreen
        loading={{ label: "Loading list…" }}
        error={{
          message: "This list could not be loaded.",
          onRetry: () => void Promise.all([lists.refetch(), games.refetch()]),
        }}
        empty={{ title: "List not found", message: "This list is no longer available." }}
      />
    );
  }

  const toggleVisibility = async () => {
    const nextVisibility = list.visibility === "Private" ? "Public" : "Private";
    await update.mutateAsync({
      path: { id },
      body: {
        name: list.name,
        description: null,
        visibility: nextVisibility,
      },
    });
    posthog?.capture("game_list_visibility_changed", {
      list_id: id,
      visibility: nextVisibility.toLowerCase(),
    });
    await queryClient.invalidateQueries({ queryKey: getApiMeListsQueryKey() });
  };

  return (
    <View style={[styles.screen, { backgroundColor: colors.background }]}>
      <View style={styles.heading}>
        <Text style={[styles.title, { color: colors.text }]}>{list.name}</Text>
        <Text
          style={[styles.meta, { color: colors.textMuted }]}
        >{`${list.visibility} · ${list.itemCount} games`}</Text>
      </View>
      <ContentState
        status={gameItems.length ? "ready" : "empty"}
        empty={{ title: "No games yet", message: "Save games from a game page to add them here." }}
        minHeight={160}
      >
        <View style={styles.games}>
          {gameItems.map((game, index) => {
            const hero = gameDetails[index]?.data?.game;
            const href = {
              pathname: "/(discover)/games/[slug]",
              params: { slug: game.gameId },
            } satisfies Href;
            return (
              <View
                key={game.gameId}
                style={[
                  commonStyles.surfaceCard,
                  styles.gameRow,
                  { backgroundColor: colors.surfaceHigh },
                ]}
              >
                <Link href={href} asChild>
                  <Pressable style={[commonStyles.row, styles.gameLink]}>
                    {hero?.cover ? (
                      <Image
                        source={getIgdbImageUrl(hero.cover, "cover_small")}
                        style={styles.cover}
                        contentFit="cover"
                      />
                    ) : (
                      <View style={[styles.cover, { backgroundColor: colors.surface }]} />
                    )}
                    <View style={styles.gameCopy}>
                      <Text style={[styles.gameName, { color: colors.text }]} numberOfLines={2}>
                        {hero?.name ?? "Loading game…"}
                      </Text>
                      <Text style={[styles.meta, { color: colors.textMuted }]}>
                        {new Date(game.addedAt).toLocaleDateString()}
                      </Text>
                    </View>
                  </Pressable>
                </Link>
                <Pressable
                  accessibilityRole="button"
                  accessibilityLabel={`Remove ${hero?.name ?? "game"} from ${list.name}`}
                  disabled={removeGame.isPending}
                  onPress={() => void deleteGame(game.gameId)}
                  style={styles.removeGame}
                >
                  <Text style={[styles.removeText, { color: colors.textMuted }]}>Remove</Text>
                </Pressable>
              </View>
            );
          })}
        </View>
      </ContentState>
      {!list.isDefault ? (
        <View style={styles.actions}>
          <Pressable
            disabled={update.isPending}
            onPress={() => void toggleVisibility()}
            style={[styles.action, { borderColor: colors.outline }]}
          >
            <Text style={[styles.deleteText, { color: colors.text }]}>
              {list.visibility === "Private" ? "Make public" : "Make private"}
            </Text>
          </Pressable>
          <Pressable
            disabled={remove.isPending}
            onPress={() => void deleteList()}
            style={[styles.action, { borderColor: colors.outline }]}
          >
            <Text style={[styles.deleteText, { color: colors.text }]}>Delete list</Text>
          </Pressable>
        </View>
      ) : null}
    </View>
  );
}

const styles = StyleSheet.create({
  screen: { flex: 1, gap: 18, padding: 20 },
  heading: { gap: 4 },
  title: { fontSize: 28, fontWeight: "800" },
  meta: { fontSize: 14, fontWeight: "600" },
  games: { gap: 8 },
  gameRow: {
    minHeight: 86,
    padding: 10,
  },
  gameLink: { flex: 1 },
  cover: { aspectRatio: 0.72, borderRadius: 8, height: 66 },
  gameCopy: { flex: 1, gap: 4 },
  gameName: { fontSize: 16, fontWeight: "700" },
  removeGame: { minHeight: 40, justifyContent: "center", paddingHorizontal: 8 },
  removeText: { fontSize: 13, fontWeight: "800" },
  actions: { gap: 10 },
  action: {
    alignItems: "center",
    borderRadius: 24,
    borderWidth: 1,
    justifyContent: "center",
    minHeight: 46,
  },
  deleteText: { fontSize: 15, fontWeight: "800" },
});
