import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useRouter } from "expo-router";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { authStorage } from "@/features/profile/auth-storage";
import { sessionTokenQueryKey } from "@/features/profile/auth-query";
import {
  getApiMeGamesGameidListsQueryKey,
  getApiMeListsQueryKey,
  useDeleteApiMeListsIdGamesGameid,
  useDeleteApiMeWishlistGamesGameid,
  useGetApiMeGamesGameidLists,
  useGetApiMeLists,
  usePutApiMeListsIdGamesGameid,
  usePutApiMeWishlistGamesGameid,
} from "@/gen/play-api";
import { useAppTheme } from "@/hooks/use-app-theme";
import { posthog } from "@/lib/posthog";
import { commonStyles } from "@/shared/ui/common-styles";

export function GameListActions({ gameId }: { gameId: string }) {
  const colors = useAppTheme();
  const router = useRouter();
  const queryClient = useQueryClient();
  const token = useQuery({
    queryKey: sessionTokenQueryKey,
    queryFn: authStorage.get,
    staleTime: Infinity,
  });
  const signedIn = typeof token.data === "string";
  const membership = useGetApiMeGamesGameidLists(
    { path: { gameId } },
    { query: { enabled: signedIn, retry: false } },
  );
  const lists = useGetApiMeLists({ query: { enabled: signedIn, retry: false } });
  const addWishlist = usePutApiMeWishlistGamesGameid();
  const removeWishlist = useDeleteApiMeWishlistGamesGameid();
  const addToList = usePutApiMeListsIdGamesGameid();
  const removeFromList = useDeleteApiMeListsIdGamesGameid();

  const invalidate = async () => {
    await Promise.all([
      queryClient.invalidateQueries({
        queryKey: getApiMeGamesGameidListsQueryKey({ path: { gameId } }),
      }),
      queryClient.invalidateQueries({ queryKey: getApiMeListsQueryKey() }),
    ]);
  };

  const toggleWishlist = async () => {
    if (!signedIn) {
      router.push("/profile");
      return;
    }
    if (membership.data?.isWishlisted) {
      await removeWishlist.mutateAsync({ path: { gameId } });
      posthog?.capture("game_removed_from_wishlist", { game_id: gameId });
    } else {
      await addWishlist.mutateAsync({ path: { gameId } });
      posthog?.capture("game_added_to_wishlist", { game_id: gameId });
    }
    await invalidate();
  };

  const toggleList = async (id: string) => {
    if (membership.data?.listIds.includes(id)) {
      await removeFromList.mutateAsync({ path: { id, gameId } });
      posthog?.capture("game_removed_from_list", { game_id: gameId, list_id: id });
    } else {
      await addToList.mutateAsync({ path: { id, gameId } });
      posthog?.capture("game_added_to_list", { game_id: gameId, list_id: id });
    }
    await invalidate();
  };

  const pending =
    addWishlist.isPending ||
    removeWishlist.isPending ||
    addToList.isPending ||
    removeFromList.isPending;

  return (
    <View style={styles.container}>
      <Pressable
        accessibilityRole="button"
        accessibilityLabel={
          membership.data?.isWishlisted ? "Remove from wishlist" : "Add to wishlist"
        }
        disabled={pending}
        onPress={() => void toggleWishlist()}
        style={[
          styles.primary,
          { backgroundColor: colors.primaryContainer, opacity: pending ? 0.65 : 1 },
        ]}
      >
        <Text style={[styles.primaryText, { color: colors.onPrimaryContainer }]}>
          {membership.data?.isWishlisted ? "Wishlisted" : signedIn ? "Wishlist" : "Sign in to save"}
        </Text>
      </Pressable>
      {signedIn && lists.data?.length ? (
        <View style={[styles.listBox, { backgroundColor: colors.surfaceHigh }]}>
          <Text style={[styles.listTitle, { color: colors.text }]}>Save to a list</Text>
          {lists.data.map((list) => {
            const selected = membership.data?.listIds.includes(list.id) ?? false;
            return (
              <Pressable
                key={list.id}
                disabled={pending}
                onPress={() => void toggleList(list.id)}
                style={[commonStyles.row, styles.listRow]}
              >
                <Text style={[styles.listName, { color: colors.text }]}>{list.name}</Text>
                <Text
                  style={[
                    styles.listState,
                    { color: selected ? colors.primary : colors.textMuted },
                  ]}
                >
                  {selected ? "Saved" : "Save"}
                </Text>
              </Pressable>
            );
          })}
        </View>
      ) : null}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { gap: 12 },
  primary: {
    alignItems: "center",
    borderRadius: 24,
    minHeight: 48,
    justifyContent: "center",
    paddingHorizontal: 20,
  },
  primaryText: { fontSize: 15, fontWeight: "800" },
  listBox: { borderRadius: 18, gap: 2, overflow: "hidden", padding: 8 },
  listTitle: { fontSize: 15, fontWeight: "800", paddingHorizontal: 8, paddingVertical: 6 },
  listRow: { minHeight: 44, paddingHorizontal: 8 },
  listName: { flex: 1, fontSize: 15, fontWeight: "600" },
  listState: { fontSize: 13, fontWeight: "800" },
});
