import { useQueryClient } from "@tanstack/react-query";
import { Button as UiButton, Host } from "@expo/ui";
import {
  Button,
  Checkbox,
  Column,
  ListItem,
  ModalBottomSheet,
  Row,
  Text,
} from "@expo/ui/jetpack-compose";
import { clickable, fillMaxWidth, paddingAll, weight } from "@expo/ui/jetpack-compose/modifiers";
import { useRouter } from "expo-router";
import { useState } from "react";

import { useAuth } from "@/features/profile";
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
import { posthog } from "@/lib/posthog";

export function GameListActions({ gameId }: { gameId: string }) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const { isAuthenticated } = useAuth();
  const [sheetVisible, setSheetVisible] = useState(false);
  const signedIn = isAuthenticated;
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
    <Host matchContents={{ vertical: true }} style={{ width: "100%" }}>
      <Row modifiers={[fillMaxWidth()]} horizontalArrangement={{ spacedBy: 10 }}>
        <Button enabled={!pending} modifiers={[weight(1)]} onClick={() => void toggleWishlist()}>
          <Text>
            {membership.data?.isWishlisted
              ? "Wishlisted"
              : signedIn
                ? "Wishlist"
                : "Sign in to save"}
          </Text>
        </Button>
        {signedIn ? (
          <Button enabled={!pending} modifiers={[weight(1)]} onClick={() => setSheetVisible(true)}>
            <Text>Save to list</Text>
          </Button>
        ) : null}
      </Row>
      {sheetVisible ? (
        <ModalBottomSheet onDismissRequest={() => setSheetVisible(false)}>
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
