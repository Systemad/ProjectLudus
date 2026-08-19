import {
  useGamesGetHero,
  useGamesGetLinks,
  useGamesGetMedia,
  useGamesGetOverview,
  useGamesGetSimilar,
} from "@/gen/hooks/GamesHooks";
import { useSteamGetPricing, useSteamGetReviews } from "@/gen/hooks/SteamHooks";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { GameHeroDto } from "@/gen/types/GameHeroDto";
import type { GameMediaDto } from "@/gen/types/GameMediaDto";
import type { GameOverviewDto } from "@/gen/types/GameOverviewDto";
import type { GetGameMediaResponse } from "@/gen/types/GetGameMediaResponse";
import type { GetGameOverviewResponse } from "@/gen/types/GetGameOverviewResponse";
import type { GetGameHeroResponse } from "@/gen/types/GetGameHeroResponse";
import type { GetGameLinksResponse } from "@/gen/types/GetGameLinksResponse";
import type { GetSimilarGamesResponse } from "@/gen/types/GetSimilarGamesResponse";
import type { WebsiteDto } from "@/gen/types/WebsiteDto";

const selectHero = (response: GetGameHeroResponse): GameHeroDto => response.game;
const selectLinks = (response: GetGameLinksResponse): WebsiteDto[] => response.websites;
const selectOverview = (response: GetGameOverviewResponse): GameOverviewDto => response.game;
const selectMedia = (response: GetGameMediaResponse): GameMediaDto => response.game;
const selectSimilar = (response: GetSimilarGamesResponse): GameBrowseDto[] => response.games;

export function useGameDetailData(gameId: string) {
  const heroQuery = useGamesGetHero<GameHeroDto>(
    { path: { gameId } },
    { query: { select: selectHero } },
  );
  const overviewQuery = useGamesGetOverview<GameOverviewDto>(
    { path: { gameId } },
    { query: { select: selectOverview } },
  );
  const similarQuery = useGamesGetSimilar<GameBrowseDto[]>(
    { path: { gameId } },
    { query: { select: selectSimilar } },
  );
  const mediaQuery = useGamesGetMedia<GameMediaDto>(
    { path: { gameId } },
    { query: { select: selectMedia } },
  );
  const linksQuery = useGamesGetLinks<WebsiteDto[]>(
    { path: { gameId } },
    { query: { retry: false, select: selectLinks } },
  );

  const steamAvailable = overviewQuery.isSuccess && overviewQuery.data.steam != null;
  const reviewsQuery = useSteamGetReviews(
    { path: { gameId } },
    { query: { enabled: steamAvailable, retry: false } },
  );
  const pricingQuery = useSteamGetPricing(
    { path: { gameId } },
    { query: { enabled: steamAvailable, retry: false } },
  );

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
