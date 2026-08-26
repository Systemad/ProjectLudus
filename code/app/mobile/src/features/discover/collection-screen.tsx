import { Stack, useLocalSearchParams } from "expo-router";

import { getGameCardItem } from "@/entities/game/game-card";
import { GameGrid } from "@/entities/game/game-grid";
import { useCalendarGetGames } from "@/gen/hooks/CalendarHooks";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useIgdbGetMostAnticipated, useIgdbGetPopscore } from "@/gen/hooks/IGDBHooks";
import { useSteamChart } from "@/gen/hooks/SteamHooks";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { useContentBottomInset } from "@/shared/ui/insets";
import { parseYearParam } from "@/utils/search-params";
import { getGameDetailHref } from "@/utils/game-routes";
import type { Href } from "expo-router";

const DEFAULT_RELEASE_YEAR = 2026;

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
  const query = useIgdbGetPopscore({
    query: {
      PopularityTypeId: String(9),
      Page: 1,
      PageSize: 15,
    },
  });

  return (
    <>
      <Stack.Screen options={{ title: "Trending" }} />
      <CollectionGrid
        getHref={(game) => getGameDetailHref(game.id)}
        games={query.data?.games ?? []}
        isLoading={query.isLoading}
        isError={query.isError}
        onRetry={() => void query.refetch()}
      />
    </>
  );
}

function ComingUpCollection() {
  const query = useIgdbGetMostAnticipated({
    query: {
      Page: 1,
      PageSize: 15,
    },
  });

  return (
    <>
      <Stack.Screen options={{ title: "Coming up" }} />
      <CollectionGrid
        getHref={(game) => getGameDetailHref(game.id)}
        games={query.data?.games ?? []}
        isLoading={query.isLoading}
        isError={query.isError}
        onRetry={() => void query.refetch()}
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
  const query = useSteamChart({ query: { Type: type, Page: 1, PageSize: 15 } });

  return (
    <>
      <Stack.Screen options={{ title }} />
      <CollectionGrid
        getHref={(game) => getGameDetailHref(game.id)}
        games={query.data?.games ?? []}
        isLoading={query.isLoading}
        isError={query.isError}
        onRetry={() => void query.refetch()}
      />
    </>
  );
}

function ReleasedCollection({ year }: { year: number }) {
  const query = useCalendarGetGames({
    path: { year },
    query: {
      Page: 1,
      PageSize: 15,
    },
  });

  return (
    <>
      <Stack.Screen options={{ title: `Released in ${year}` }} />
      <CollectionGrid
        getHref={(game) => getGameDetailHref(game.id)}
        games={query.data?.games ?? []}
        isLoading={query.isLoading}
        isError={query.isError}
        onRetry={() => void query.refetch()}
      />
    </>
  );
}

function CollectionGrid({
  games,
  getHref,
  isLoading,
  isError,
  onRetry,
}: {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
}) {
  const bottomInset = useContentBottomInset(120);

  return (
    <ContentState
      fullScreen
      status={getContentStateStatus(isLoading, isError, games.length === 0)}
      loading={{ label: "Loading games…" }}
      error={{ onRetry, title: "Couldn’t load this collection." }}
      empty={{ title: "No games found", message: "There are no games in this collection yet." }}
    >
      <GameGrid
        items={games.map((game) => getGameCardItem(game, getHref(game)))}
        bottomInset={bottomInset}
      />
    </ContentState>
  );
}
