import { type Href } from "expo-router";
import { ScrollView, StyleSheet } from "react-native";

import { DiscoverRail } from "@/features/discover/discover-rail";
import { LastVisitedSection } from "@/features/last-visited";
import { useIgdbGetMostAnticipated, useIgdbGetPopscore } from "@/gen/hooks/IGDBHooks";
import { useSteamChart } from "@/gen/hooks/SteamHooks";
import { useAppTheme } from "@/hooks/use-app-theme";
import { commonStyles } from "@/shared/ui/common-styles";
import { useContentBottomInset } from "@/shared/ui/insets";
import { getGameDetailHref } from "@/utils/game-routes";

const trendingHref = {
  pathname: "/(discover)/collections/[collection]",
  params: { collection: "trending" },
} satisfies Href;

const mostPlayedHref = {
  pathname: "/(discover)/collections/[collection]",
  params: { collection: "most-played" },
} satisfies Href;

const comingUpHref = {
  pathname: "/(discover)/collections/[collection]",
  params: { collection: "coming-up" },
} satisfies Href;

export default function DiscoverScreen() {
  const colors = useAppTheme();
  const bottomInset = useContentBottomInset(24);
  const trending = useIgdbGetPopscore({
    query: { PopularityTypeId: String(9), Page: 1, PageSize: 12 },
  });
  const mostPlayed = useSteamChart({
    query: { Type: "most-played", Page: 1, PageSize: 12 },
  });
  const comingUp = useIgdbGetMostAnticipated({
    query: { Page: 1, PageSize: 12 },
  });

  return (
    <ScrollView
      style={{ backgroundColor: colors.background }}
      contentContainerStyle={[
        commonStyles.pageGutter,
        styles.content,
        { paddingBottom: bottomInset },
      ]}
      contentInsetAdjustmentBehavior="automatic"
    >
      <DiscoverRail
        title="Trending"
        subtitle="Global top sellers right now"
        href={trendingHref}
        getGameHref={(game) => getGameDetailHref(game.id)}
        games={trending.data?.games ?? []}
        isLoading={trending.isLoading}
        isError={trending.isError}
        onRetry={() => void trending.refetch()}
      />
      <DiscoverRail
        title="Most played on Steam"
        subtitle="Games with the most active players"
        href={mostPlayedHref}
        getGameHref={(game) => getGameDetailHref(game.id)}
        games={mostPlayed.data?.games ?? []}
        isLoading={mostPlayed.isLoading}
        isError={mostPlayed.isError}
        onRetry={() => void mostPlayed.refetch()}
      />
      <DiscoverRail
        title="Coming up"
        subtitle="Most anticipated games in the next 30 days"
        href={comingUpHref}
        getGameHref={(game) => getGameDetailHref(game.id)}
        games={comingUp.data?.games ?? []}
        isLoading={comingUp.isLoading}
        isError={comingUp.isError}
        onRetry={() => void comingUp.refetch()}
      />
      <LastVisitedSection />
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  content: {
    paddingTop: 12,
    gap: 24,
  },
});
