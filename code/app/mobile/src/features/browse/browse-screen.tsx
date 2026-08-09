import { useState } from "react";
import { Host } from "@expo/ui";

import { useIgdbGetPopscore } from "@/gen/hooks/IGDBHooks";
import { useSteamChart } from "@/gen/hooks/SteamHooks";
import { useAppTheme } from "@/hooks/use-app-theme";

import { BrowseList, type BrowseCollection } from "./browse-list.android";

export default function BrowseScreen() {
  const colors = useAppTheme();
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

  return (
    <Host style={{ flex: 1, backgroundColor: colors.background }}>
      <BrowseList
        collection={collection}
        games={selected.data?.games ?? []}
        isLoading={selected.isLoading}
        isError={selected.isError}
        onRetry={() => void selected.refetch()}
        onCollectionChange={setCollection}
      />
    </Host>
  );
}
