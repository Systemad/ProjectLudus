import { useQuery } from "@tanstack/react-query";

import {
  gamesGetHeroQueryOptions,
  gamesGetLinksQueryOptions,
  gamesGetMediaQueryOptions,
  gamesGetOverviewQueryOptions,
  gamesGetSimilarQueryOptions,
} from "@/gen/hooks/GamesHooks";
import { steamGetPricingQueryOptions, steamGetReviewsQueryOptions } from "@/gen/hooks/SteamHooks";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { GameHeroDto } from "@/gen/types/GameHeroDto";
import type { GameMediaDto } from "@/gen/types/GameMediaDto";
import type { GameOverviewDto } from "@/gen/types/GameOverviewDto";
import type { GetGameHeroResponse } from "@/gen/types/GetGameHeroResponse";
import type { GetGameLinksResponse } from "@/gen/types/GetGameLinksResponse";
import type { GetGameMediaResponse } from "@/gen/types/GetGameMediaResponse";
import type { GetGameOverviewResponse } from "@/gen/types/GetGameOverviewResponse";
import type { GetSimilarGamesResponse } from "@/gen/types/GetSimilarGamesResponse";
import type { WebsiteDto } from "@/gen/types/WebsiteDto";

const selectHero = (response: GetGameHeroResponse): GameHeroDto => response.game;
const selectLinks = (response: GetGameLinksResponse): WebsiteDto[] => response.websites;
const selectOverview = (response: GetGameOverviewResponse): GameOverviewDto => response.game;
const selectMedia = (response: GetGameMediaResponse): GameMediaDto => response.game;
const selectSimilar = (response: GetSimilarGamesResponse): GameBrowseDto[] => response.games;

type GameDetailTab = "overview" | "media" | "links";

type GameDetailViewModelOptions = {
  activeTab?: GameDetailTab;
};

export function useGameDetailViewModel(
  gameId: string,
  { activeTab = "overview" }: GameDetailViewModelOptions = {},
) {
  const loadSimilar = activeTab === "overview";
  const loadMedia = activeTab === "media";
  const loadLinks = activeTab === "links";

  const heroQuery = useQuery({
    ...gamesGetHeroQueryOptions({ path: { gameId } }),
    select: selectHero,
  });
  const overviewQuery = useQuery({
    ...gamesGetOverviewQueryOptions({ path: { gameId } }),
    select: selectOverview,
  });
  const similarQuery = useQuery({
    ...gamesGetSimilarQueryOptions({ path: { gameId } }),
    enabled: loadSimilar,
    select: selectSimilar,
  });
  const mediaQuery = useQuery({
    ...gamesGetMediaQueryOptions({ path: { gameId } }),
    enabled: loadMedia,
    select: selectMedia,
  });
  const linksQuery = useQuery({
    ...gamesGetLinksQueryOptions({ path: { gameId } }),
    enabled: loadLinks,
    retry: false,
    select: selectLinks,
  });

  const steamAvailable = overviewQuery.isSuccess && overviewQuery.data.steam != null;
  const reviewsQuery = useQuery({
    ...steamGetReviewsQueryOptions({ path: { gameId } }),
    enabled: steamAvailable,
    retry: false,
  });
  const pricingQuery = useQuery({
    ...steamGetPricingQueryOptions({ path: { gameId } }),
    enabled: steamAvailable,
    retry: false,
  });

  return {
    hero: heroQuery.data,
    overview: overviewQuery.data,
    similar: similarQuery.data ?? [],
    screenshots: mediaQuery.data?.screenshots ?? [],
    videos: mediaQuery.data?.videos ?? [],
    websites: linksQuery.data ?? [],
    steam: overviewQuery.data?.steam,
    reviews: reviewsQuery.data,
    pricing: pricingQuery.data,
    isCorePending: heroQuery.isPending || overviewQuery.isPending,
    hasCoreError: heroQuery.isError || overviewQuery.isError,
    isSimilarPending: similarQuery.isPending,
    hasSimilarError: similarQuery.isError,
    isMediaPending: mediaQuery.isPending,
    hasMediaError: mediaQuery.isError,
    isLinksPending: linksQuery.isPending,
    hasLinksError: linksQuery.isError,
    retryCore: () => Promise.all([heroQuery.refetch(), overviewQuery.refetch()]),
    retrySimilar: () => similarQuery.refetch(),
    retryMedia: () => mediaQuery.refetch(),
    retryLinks: () => linksQuery.refetch(),
  };
}
