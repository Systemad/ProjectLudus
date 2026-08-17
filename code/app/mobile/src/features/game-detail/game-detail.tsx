import { useEffect } from "react";
import { Text, View } from "react-native";
import type { Href } from "expo-router";

import { GameCarousel } from "@/entities/game/game-carousel";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { CompanyList } from "@/features/game-detail/company-list";
import { GameDetailShell } from "@/features/game-detail/game-detail-shell";
import { GameFactGrid } from "@/features/game-detail/game-fact-grid";
import { GameScreenshotGallery } from "@/features/game-detail/game-screenshot-gallery";
import { SteamChart } from "@/features/game-detail/steam-chart.android";
import { SteamSummary } from "@/features/game-detail/steam-summary";
import { GameListActions } from "@/features/lists/game-list-actions";
import { useLastVisited } from "@/features/last-visited";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { detailStyles } from "@/shared/ui/detail-shell";
import { useGameDetailData } from "./use-game-detail-data";

const getRelatedGameHref = (game: { id: string | number }) =>
  ({
    pathname: "./[slug]",
    params: { slug: String(game.id) },
  }) satisfies Href;

export function GameDetail({ slug }: { slug: string }) {
  const colors = useAppTheme();
  const gameId = String(slug);
  const { remember } = useLastVisited();
  const game = useGameDetailData(gameId);
  const status = getContentStateStatus(
    game.isCorePending,
    game.hasCoreError,
    !game.hero || !game.overview,
  );

  useEffect(() => {
    remember(gameId);
  }, [gameId, remember]);

  if (!game.hero || !game.overview || status !== "ready") {
    return (
      <ContentState
        status={status}
        fullScreen
        loading={{ label: "Loading game…" }}
        error={{ onRetry: () => void game.retryCore() }}
        empty={{ title: "Game not found", message: "The API did not return this game." }}
      />
    );
  }

  const companies = game.hero.companies;
  const relatedStatus = getContentStateStatus(
    game.isSimilarPending,
    game.hasSimilarError,
    game.similar.length === 0,
  );
  const mediaStatus = getContentStateStatus(
    game.isMediaPending,
    game.hasMediaError,
    game.screenshots.length === 0,
  );
  const showRelated = relatedStatus !== "empty";

  return (
    <GameDetailShell
      title={game.hero.name}
      eyebrow={`${game.hero.firstReleaseDate?.slice(0, 4) ?? "TBA"} · ${game.hero.gameTypeName ?? "Game"}`}
      summary={game.hero.summary ?? game.overview.storyline ?? "No summary is available yet."}
      imageUrl={getIgdbImageUrl(game.hero.cover, "cover_big", true)}
    >
      <GameListActions gameId={gameId} />
      <SteamSummary steam={game.steam} reviews={game.reviews} pricing={game.pricing} />
      <GameFactGrid
        facts={[
          {
            label: "Platforms",
            values: game.hero.platforms.map((item) => item.name),
            wide: true,
          },
          { label: "Genres", values: game.hero.genres.map((item) => item.name) },
          { label: "Themes", values: game.hero.themes.map((item) => item.name) },
          { label: "Game modes", values: game.hero.gameModes.map((item) => item.name) },
          {
            label: "Player perspective",
            values: game.hero.playerPerspectives.map((item) => item.name),
          },
        ]}
      />
      <View style={detailStyles.section}>
        <Text style={[detailStyles.sectionTitle, { color: colors.text }]}>Screenshots</Text>
        <ContentState
          status={mediaStatus}
          minHeight={120}
          loading={{ label: "Loading screenshots…" }}
          error={{
            message: "Screenshots could not be loaded.",
            onRetry: () => void game.retryMedia(),
          }}
          empty={{
            title: "No screenshots available",
            message: "This game does not have screenshots yet.",
          }}
        >
          <GameScreenshotGallery screenshotIds={game.screenshots} />
        </ContentState>
      </View>
      {companies.length ? (
        <View style={detailStyles.section}>
          <Text style={[detailStyles.sectionTitle, { color: colors.text }]}>Companies</Text>
          <CompanyList key={gameId} companies={companies} />
        </View>
      ) : null}
      <SteamChart gameId={gameId} />
      {showRelated ? (
        <View style={detailStyles.section}>
          <Text style={[detailStyles.sectionTitle, { color: colors.text }]}>Related games</Text>
          <ContentState
            status={relatedStatus}
            minHeight={120}
            loading={{ label: "Loading related games…" }}
            error={{
              message: "Related games could not be loaded.",
              onRetry: () => void game.retrySimilar(),
            }}
          >
            <GameCarousel games={game.similar} getHref={getRelatedGameHref} variant="cover" />
          </ContentState>
        </View>
      ) : null}
    </GameDetailShell>
  );
}
