import { Stack, useLocalSearchParams } from "expo-router";

import { GameGrid } from "@/entities/game/game-grid";
import { useCalendarGetGamesInfinite } from "@/gen/hooks/CalendarHooks";
import {
  useIgdbGetMostAnticipatedInfinite,
  useIgdbGetPopscoreInfinite,
} from "@/gen/hooks/IGDBHooks";
import { useSteamChartInfinite } from "@/gen/hooks/SteamHooks";
import { parseYearParam } from "@/utils/search-params";
import type { Href } from "expo-router";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";

const DEFAULT_RELEASE_YEAR = 2026;

const getDiscoverGameHref = (game: GameBrowseDto) =>
  ({
    pathname: "../games/[slug]",
    params: { slug: String(game.id) },
  }) satisfies Href;

export default function CollectionScreen() {
  const { collection, year } = useLocalSearchParams<{
    collection: string;
    year?: string | string[];
  }>();

  switch (collection) {
    case "trending":
      return <TrendingCollection />;
    case "most-played":
      return <SteamCollection title="Most played on Steam" type="most-played" />;
    case "popular-releases":
      return <SteamCollection title="Popular releases" type="popular-releases" />;
    case "hot-releases":
      return <SteamCollection title="Hot releases" type="hot-releases" />;
    case "coming-up":
      return <ComingUpCollection />;
    case "released":
      return <ReleasedCollection year={parseYearParam(year, DEFAULT_RELEASE_YEAR)} />;
    default:
      return <Stack.Screen options={{ title: "Collection not found" }} />;
  }
}

function TrendingCollection() {
  const query = useIgdbGetPopscoreInfinite({
    query: {
      PopularityTypeId: String(9),
      Page: 1,
      PageSize: 20,
    },
  });

  return (
    <>
      <Stack.Screen options={{ title: "Trending" }} />
      <GameGrid
        getHref={getDiscoverGameHref}
        games={query.data?.pages.flatMap((page) => page.games) ?? []}
        isLoading={query.isLoading}
        isError={query.isError}
        isFetchingNextPage={query.isFetchingNextPage}
        isFetchNextPageError={query.isFetchNextPageError}
        hasNextPage={query.hasNextPage}
        onLoadMore={() => void query.fetchNextPage()}
        onRetry={() => void query.refetch()}
        onRetryNextPage={() => void query.fetchNextPage()}
      />
    </>
  );
}

function ComingUpCollection() {
  const query = useIgdbGetMostAnticipatedInfinite({
    query: {
      Page: 1,
      PageSize: 20,
    },
  });

  return (
    <>
      <Stack.Screen options={{ title: "Coming up" }} />
      <GameGrid
        getHref={getDiscoverGameHref}
        games={query.data?.pages.flatMap((page) => page.games) ?? []}
        isLoading={query.isLoading}
        isError={query.isError}
        isFetchingNextPage={query.isFetchingNextPage}
        isFetchNextPageError={query.isFetchNextPageError}
        hasNextPage={query.hasNextPage}
        onLoadMore={() => void query.fetchNextPage()}
        onRetry={() => void query.refetch()}
        onRetryNextPage={() => void query.fetchNextPage()}
      />
    </>
  );
}

function SteamCollection({
  title,
  type,
}: {
  title: string;
  type: "most-played" | "popular-releases" | "hot-releases";
}) {
  const query = useSteamChartInfinite({ query: { Type: type, Page: 1, PageSize: 20 } });

  return (
    <>
      <Stack.Screen options={{ title }} />
      <GameGrid
        getHref={getDiscoverGameHref}
        games={query.data?.pages.flatMap((page) => page.games) ?? []}
        isLoading={query.isLoading}
        isError={query.isError}
        isFetchingNextPage={query.isFetchingNextPage}
        isFetchNextPageError={query.isFetchNextPageError}
        hasNextPage={query.hasNextPage}
        onLoadMore={() => void query.fetchNextPage()}
        onRetry={() => void query.refetch()}
        onRetryNextPage={() => void query.fetchNextPage()}
      />
    </>
  );
}

function ReleasedCollection({ year }: { year: number }) {
  const query = useCalendarGetGamesInfinite({
    path: { year },
    query: {
      Page: 1,
      PageSize: 20,
    },
  });

  return (
    <>
      <Stack.Screen options={{ title: `Released in ${year}` }} />
      <GameGrid
        getHref={getDiscoverGameHref}
        games={query.data?.pages.flatMap((page) => page.games) ?? []}
        isLoading={query.isLoading}
        isError={query.isError}
        isFetchingNextPage={query.isFetchingNextPage}
        isFetchNextPageError={query.isFetchNextPageError}
        hasNextPage={query.hasNextPage}
        onLoadMore={() => void query.fetchNextPage()}
        onRetry={() => void query.refetch()}
        onRetryNextPage={() => void query.fetchNextPage()}
      />
    </>
  );
}
