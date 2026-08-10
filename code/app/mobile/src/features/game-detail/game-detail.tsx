import { useQuery } from "@tanstack/react-query";
import { Text, View } from "react-native";
import type { Href } from "expo-router";

import { GameCarousel } from "@/entities/game/game-carousel";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { CompanyList } from "@/features/game-detail/company-list";
import { GameDetailShell } from "@/features/game-detail/game-detail-shell";
import { GameFactGrid } from "@/features/game-detail/game-fact-grid";
import { SteamChart } from "@/features/game-detail/steam-chart.android";
import { GameListActions } from "@/features/lists/game-list-actions";
import {
  gamesGetHeroQueryOptions,
  gamesGetOverviewQueryOptions,
  gamesGetSimilarQueryOptions,
} from "@/gen/hooks/GamesHooks";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { detailStyles } from "@/shared/ui/detail-shell";

const getRelatedGameHref = (game: { id: string | number }) =>
  ({
    pathname: "./[slug]",
    params: { slug: String(game.id) },
  }) satisfies Href;

export function GameDetail({ slug }: { slug: string }) {
  const colors = useAppTheme();
  const gameId = String(slug);
  const heroQuery = useQuery(gamesGetHeroQueryOptions({ path: { gameId } }));
  const overviewQuery = useQuery(gamesGetOverviewQueryOptions({ path: { gameId } }));
  const similarQuery = useQuery(gamesGetSimilarQueryOptions({ path: { gameId } }));
  const hero = heroQuery.data?.game;
  const overview = overviewQuery.data?.game;
  const isLoading = heroQuery.isLoading || overviewQuery.isLoading;
  const isError = heroQuery.isError || overviewQuery.isError;
  const status = getContentStateStatus(isLoading, isError, !hero || !overview);

  if (!hero || !overview || status !== "ready") {
    return (
      <ContentState
        status={status}
        fullScreen
        loading={{ label: "Loading game…" }}
        error={{ onRetry: () => void Promise.all([heroQuery.refetch(), overviewQuery.refetch()]) }}
        empty={{ title: "Game not found", message: "The API did not return this game." }}
      />
    );
  }

  const companies = hero.companies;
  const similar = similarQuery.data?.games ?? [];
  const relatedStatus = getContentStateStatus(
    similarQuery.isLoading,
    similarQuery.isError,
    similar.length === 0,
  );
  const showRelated = relatedStatus !== "empty";

  return (
    <GameDetailShell
      title={hero.name}
      eyebrow={`${hero.firstReleaseDate?.slice(0, 4) ?? "TBA"} · ${hero.gameTypeName ?? "Game"}`}
      summary={hero.summary ?? overview.storyline ?? "No summary is available yet."}
      imageUrl={getIgdbImageUrl(hero.cover, "cover_big", true)}
    >
      <GameFactGrid
        facts={[
          {
            label: "Platforms",
            values: hero.platforms.map((item) => item.name),
            wide: true,
          },
          { label: "Genres", values: hero.genres.map((item) => item.name) },
          { label: "Themes", values: hero.themes.map((item) => item.name) },
          { label: "Game modes", values: hero.gameModes.map((item) => item.name) },
          {
            label: "Player perspective",
            values: hero.playerPerspectives.map((item) => item.name),
          },
        ]}
      />
      <View style={detailStyles.section}>
        <GameListActions gameId={gameId} />
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
              onRetry: () => void similarQuery.refetch(),
            }}
          >
            <GameCarousel games={similar} getHref={getRelatedGameHref} variant="cover" />
          </ContentState>
        </View>
      ) : null}
    </GameDetailShell>
  );
}
