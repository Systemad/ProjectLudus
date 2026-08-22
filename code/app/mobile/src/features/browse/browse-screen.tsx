import { useState } from "react";

import { useIgdbGetPopscore } from "@/gen/hooks/IGDBHooks";
import { useSteamChart } from "@/gen/hooks/SteamHooks";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";

import { BrowseList, type BrowseCollection } from "./browse-list";

export default function BrowseScreen() {
  const [collection, setCollection] = useState<BrowseCollection>("mostPlayed");

  const mostPlayed = useSteamChart({
    query: { Type: "most-played", Page: 1, PageSize: 20 },
  });
  const popularReleases = useSteamChart({
    query: { Type: "popular-releases", Page: 1, PageSize: 20 },
  });
  const hotReleases = useSteamChart({
    query: { Type: "hot-releases", Page: 1, PageSize: 20 },
  });
  const trending = useIgdbGetPopscore({
    query: { PopularityTypeId: "9", Page: 1, PageSize: 20 },
  });

  const selected = { mostPlayed, popularReleases, hotReleases, trending }[collection];
  const games = selected.data?.games ?? [];
  const status = getContentStateStatus(
    selected.isLoading,
    selected.isError,
    games.length === 0,
  );

  if (status !== "ready") {
    return (
      <ContentState
        fullScreen
        status={status}
        loading={{ label: "Loading games…" }}
        error={{
          onRetry: () => void selected.refetch(),
          title: "This list could not be loaded.",
          retryLabel: "Retry",
        }}
        empty={{ title: "No games found", message: "There are no games in this collection yet." }}
      />
    );
  }

  return <BrowseList collection={collection} games={games} onCollectionChange={setCollection} />;
}
