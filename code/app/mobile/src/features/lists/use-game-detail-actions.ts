import { useQueryClient } from "@tanstack/react-query";
import { useRouter } from "expo-router";
import { Share } from "react-native";

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

export function useGameDetailActions(gameId: string) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const { isAuthenticated } = useAuth();
  const membership = useGetApiMeGamesGameidLists(
    { path: { gameId } },
    { query: { enabled: isAuthenticated, retry: false } },
  );
  const lists = useGetApiMeLists({ query: { enabled: isAuthenticated, retry: false } });

  const invalidateMembership = () =>
    Promise.all([
      queryClient.invalidateQueries({
        queryKey: getApiMeGamesGameidListsQueryKey({ path: { gameId } }),
      }),
      queryClient.invalidateQueries({ queryKey: getApiMeListsQueryKey() }),
    ]);

  const addWishlist = usePutApiMeWishlistGamesGameid({
    mutation: {
      onSuccess: async () => {
        await invalidateMembership();
        posthog?.capture("game_added_to_wishlist", { game_id: gameId });
      },
    },
  });
  const removeWishlist = useDeleteApiMeWishlistGamesGameid({
    mutation: {
      onSuccess: async () => {
        await invalidateMembership();
        posthog?.capture("game_removed_from_wishlist", { game_id: gameId });
      },
    },
  });
  const addToList = usePutApiMeListsIdGamesGameid({
    mutation: {
      onSuccess: async (_, variables) => {
        await invalidateMembership();
        posthog?.capture("game_added_to_list", {
          game_id: gameId,
          list_id: variables.path.id,
        });
      },
    },
  });
  const removeFromList = useDeleteApiMeListsIdGamesGameid({
    mutation: {
      onSuccess: async (_, variables) => {
        await invalidateMembership();
        posthog?.capture("game_removed_from_list", {
          game_id: gameId,
          list_id: variables.path.id,
        });
      },
    },
  });
  const pending =
    addWishlist.isPending ||
    removeWishlist.isPending ||
    addToList.isPending ||
    removeFromList.isPending;

  const toggleWishlist = () => {
    if (!isAuthenticated) {
      router.push("/profile");
      return;
    }
    if (membership.data?.isWishlisted) {
      removeWishlist.mutate({ path: { gameId } });
    } else {
      addWishlist.mutate({ path: { gameId } });
    }
  };

  const toggleList = (listId: string) => {
    if (membership.data?.listIds.includes(listId)) {
      removeFromList.mutate({ path: { id: listId, gameId } });
    } else {
      addToList.mutate({ path: { id: listId, gameId } });
    }
  };

  return {
    signedIn: isAuthenticated,
    membership,
    lists,
    pending,
    goToProfile: () => router.push("/profile"),
    toggleWishlist,
    toggleList,
    share: () => void Share.share({ message: `Open this game in Ludus: ${gameId}` }),
    createList: () => router.push("/profile"),
  };
}
