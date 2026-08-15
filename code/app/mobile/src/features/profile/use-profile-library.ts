import type { GetApiMeLibraryHistoryStatus200 } from "@/gen/play-api/types/GetApiMeLibraryHistory";
import type { GetApiMeLibraryListsListidStatus200 } from "@/gen/play-api/types/GetApiMeLibraryListsListid";
import type { GetApiMeWishlistStatus200 } from "@/gen/play-api/types/GetApiMeWishlist";
import type { GameCard } from "@/gen/play-api/types/GameCard";

import {
  useGetApiMeLibraryHistory,
  useGetApiMeLibraryListsListid,
  useGetApiMeLists,
  useGetApiMeWishlist,
} from "@/gen/play-api";

export type ProfileLibraryTab = "wishlist" | "lists" | "history";

type ProfileCollection = {
  games: GameCard[];
  count: number;
  list: GetApiMeLibraryListsListidStatus200["list"] | null;
};

function selectHistory(response: GetApiMeLibraryHistoryStatus200): ProfileCollection {
  return {
    games: response.items.map(({ game }) => game),
    count: response.items.length,
    list: null,
  };
}

function selectLibrary(response: GetApiMeLibraryListsListidStatus200): ProfileCollection {
  return {
    games: response.games.map(({ game }) => game),
    count: response.games.length,
    list: response.list,
  };
}

function selectWishlistId(response: GetApiMeWishlistStatus200) {
  return response.id;
}

export function useProfileLibrary({
  isAuthenticated,
  tab,
  selectedListId,
}: {
  isAuthenticated: boolean;
  tab: ProfileLibraryTab;
  selectedListId: string | null;
}) {
  const isHistory = tab === "history";
  const isWishlist = tab === "wishlist";

  const wishlist = useGetApiMeWishlist<string>({
    query: {
      enabled: isAuthenticated && isWishlist,
      retry: false,
      select: selectWishlistId,
    },
  });
  const lists = useGetApiMeLists({
    query: {
      enabled: isAuthenticated && tab === "lists",
      retry: false,
    },
  });
  const firstListId = lists.data?.find((list) => !list.isDefault)?.id ?? lists.data?.[0]?.id;
  const activeListId = isWishlist ? wishlist.data : (selectedListId ?? firstListId);
  const library = useGetApiMeLibraryListsListid<ProfileCollection>(
    { path: { listId: activeListId ?? "" }, query: { PageSize: 50 } },
    {
      query: {
        enabled: isAuthenticated && !isHistory && Boolean(activeListId),
        retry: false,
        select: selectLibrary,
      },
    },
  );
  const history = useGetApiMeLibraryHistory<ProfileCollection>(
    { query: { PageSize: 30 } },
    {
      query: {
        enabled: isAuthenticated && isHistory,
        retry: false,
        select: selectHistory,
      },
    },
  );

  const active = isHistory ? history.data : library.data;
  const isLoading = isHistory
    ? history.isLoading
    : (isWishlist ? wishlist.isLoading : lists.isLoading) || library.isLoading;
  const isError = isHistory
    ? history.isError
    : (isWishlist ? wishlist.isError : lists.isError) || library.isError;

  return {
    activeListId,
    availableLists: lists.data ?? [],
    activeCollection: active,
    isLoading,
    isError,
  };
}
