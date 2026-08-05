import { useQuery, useQueryClient } from "@tanstack/react-query";
import { Button as UiButton, Host } from "@expo/ui";
import {
  Button,
  Checkbox,
  Column,
  ListItem,
  ModalBottomSheet,
  Text,
} from "@expo/ui/jetpack-compose";
import { clickable, fillMaxWidth, paddingAll } from "@expo/ui/jetpack-compose/modifiers";
import { useRouter } from "expo-router";
import { useState } from "react";

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

export function GameListActions({ gameId }: { gameId: string }) {
  const colors = useAppTheme();
  const router = useRouter();
  const queryClient = useQueryClient();
  const [sheetVisible, setSheetVisible] = useState(false);
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
  const pending =
    addWishlist.isPending ||
    removeWishlist.isPending ||
    addToList.isPending ||
    removeFromList.isPending;
  const invalidate = () =>
    Promise.all([
      queryClient.invalidateQueries({
        queryKey: getApiMeGamesGameidListsQueryKey({ path: { gameId } }),
      }),
      queryClient.invalidateQueries({ queryKey: getApiMeListsQueryKey() }),
    ]);
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

  return (
    <Host matchContents={{ vertical: true }} seedColor={colors.primary} style={{ width: "100%" }}>
      <Column modifiers={[fillMaxWidth()]} verticalArrangement={{ spacedBy: 10 }}>
        <Button
          enabled={!pending}
          modifiers={[fillMaxWidth()]}
          onClick={() => void toggleWishlist()}
        >
          <Text>
            {membership.data?.isWishlisted
              ? "Wishlisted"
              : signedIn
                ? "Wishlist"
                : "Sign in to save"}
          </Text>
        </Button>
        {signedIn ? (
          <UiButton
            disabled={pending}
            label="Save to a list"
            onPress={() => setSheetVisible(true)}
            variant="outlined"
          />
        ) : null}
      </Column>
      {sheetVisible ? (
        <ModalBottomSheet
          containerColor={colors.surface}
          contentColor={colors.text}
          onDismissRequest={() => setSheetVisible(false)}
        >
          <Column
            modifiers={[fillMaxWidth(), paddingAll(20)]}
            verticalArrangement={{ spacedBy: 8 }}
          >
            <Text style={{ typography: "titleLarge", fontWeight: "700" }}>Save to a list</Text>
            {lists.data?.map((list) => {
              const selected = membership.data?.listIds.includes(list.id) ?? false;
              return (
                <ListItem
                  key={list.id}
                  modifiers={[fillMaxWidth(), clickable(() => void toggleList(list.id))]}
                >
                  <ListItem.HeadlineContent>
                    <Text>{list.name}</Text>
                  </ListItem.HeadlineContent>
                  <ListItem.SupportingContent>
                    <Text>{`${list.itemCount} games`}</Text>
                  </ListItem.SupportingContent>
                  <ListItem.TrailingContent>
                    <Checkbox
                      enabled={!pending}
                      onCheckedChange={() => void toggleList(list.id)}
                      value={selected}
                    />
                  </ListItem.TrailingContent>
                </ListItem>
              );
            })}
            <UiButton
              label="Create new list"
              onPress={() => {
                setSheetVisible(false);
                router.push("/profile");
              }}
              variant="outlined"
            />
          </Column>
        </ModalBottomSheet>
      ) : null}
    </Host>
  );
}
