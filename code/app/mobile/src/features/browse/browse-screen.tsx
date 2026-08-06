import { ScrollView } from "react-native";
import type { Href } from "expo-router";

import { BrowseSection } from "@/features/browse/browse-section";
import { useIgdbGetPopscore } from "@/gen/hooks/IGDBHooks";
import { useSteamChart } from "@/gen/hooks/SteamHooks";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { commonStyles } from "@/shared/ui/common-styles";

const getGameHref = (game: GameBrowseDto) =>
  ({
    pathname: "/(discover)/games/[slug]",
    params: { slug: String(game.id) },
  }) satisfies Href;

const collectionHref = (collection: string) =>
  ({
    pathname: "/(discover)/collections/[collection]",
    params: { collection },
  }) satisfies Href;

export default function BrowseScreen() {
  const colors = useAppTheme();
  const mostPlayed = useSteamChart({
    query: { Type: "most-played", Page: 1, PageSize: 6 },
  });
  const popularReleases = useSteamChart({
    query: { Type: "popular-releases", Page: 1, PageSize: 6 },
  });
  const hotReleases = useSteamChart({
    query: { Type: "hot-releases", Page: 1, PageSize: 6 },
  });
  const trending = useIgdbGetPopscore({
    query: { PopularityTypeId: String(9), Page: 1, PageSize: 6 },
  });

  return (
    <ScrollView
      style={{ backgroundColor: colors.background }}
      contentContainerStyle={[
        commonStyles.pageGutter,
        { gap: 28, paddingBottom: 120, paddingTop: 12 },
      ]}
    >
      <BrowseSection
        title="Most played on Steam"
        subtitle="Games with the most active players"
        href={collectionHref("most-played")}
        games={mostPlayed.data?.games ?? []}
        isLoading={mostPlayed.isLoading}
        isError={mostPlayed.isError}
        onRetry={() => void mostPlayed.refetch()}
        getGameHref={getGameHref}
      />
      <BrowseSection
        title="Popular releases"
        subtitle="Recent releases attracting players"
        href={collectionHref("popular-releases")}
        games={popularReleases.data?.games ?? []}
        isLoading={popularReleases.isLoading}
        isError={popularReleases.isError}
        onRetry={() => void popularReleases.refetch()}
        getGameHref={getGameHref}
      />
      <BrowseSection
        title="Hot releases"
        subtitle="Recent releases with strong reception"
        href={collectionHref("hot-releases")}
        games={hotReleases.data?.games ?? []}
        isLoading={hotReleases.isLoading}
        isError={hotReleases.isError}
        onRetry={() => void hotReleases.refetch()}
        getGameHref={getGameHref}
      />
      <BrowseSection
        title="Trending"
        subtitle="Global top sellers right now"
        href={collectionHref("trending")}
        games={trending.data?.games ?? []}
        isLoading={trending.isLoading}
        isError={trending.isError}
        onRetry={() => void trending.refetch()}
        getGameHref={getGameHref}
      />
    </ScrollView>
  );
}
