import { useQuery } from "@tanstack/react-query";
import { Text, View } from "react-native";
import type { Href } from "expo-router";

import { GameCarousel } from "@/entities/game/game-carousel";
import {
  companiesGetGamesQueryOptions,
  companiesGetQueryOptions,
} from "@/gen/hooks/CompaniesHooks";
import { useAppTheme } from "@/hooks/use-app-theme";
import { DetailShell, FactGroup, detailStyles } from "@/shared/ui/detail-shell";
import { EmptyState, ErrorState, LoadingState } from "@/shared/ui/screen-state";

const getCompanyGameHref = (game: { id: string | number }) =>
  ({
    pathname: "../games/[slug]",
    params: { slug: String(game.id) },
  }) satisfies Href;

export function CompanyDetail({ slug }: { slug: string }) {
  const colors = useAppTheme();
  const companyId = String(slug);
  const companyQuery = useQuery(companiesGetQueryOptions({ path: { companyId } }));
  const gamesQuery = useQuery(companiesGetGamesQueryOptions({ path: { companyId } }));
  const isLoading = companyQuery.isLoading || gamesQuery.isLoading;
  const isError = companyQuery.isError || gamesQuery.isError;

  if (isLoading) return <LoadingState label="Loading company…" />;
  if (isError)
    return (
      <ErrorState onRetry={() => Promise.all([companyQuery.refetch(), gamesQuery.refetch()])} />
    );
  if (!companyQuery.data?.company)
    return <EmptyState title="Company not found" message="The API did not return this company." />;

  const company = companyQuery.data.company;
  const games = gamesQuery.data ?? [];
  return (
    <DetailShell
      title={company.name}
      eyebrow={company.status ?? company.slug}
      summary={company.description ?? "No company description is available yet."}
    >
      <FactGroup
        title="Founded"
        values={[
          company.startDate
            ? new Date(Number(company.startDate) * 1000).getFullYear().toString()
            : "Unknown",
        ]}
      />
      {games.length ? (
        <View style={detailStyles.section}>
          <Text style={[detailStyles.sectionTitle, { color: colors.text }]}>Games</Text>
          <GameCarousel games={games} getHref={getCompanyGameHref} />
        </View>
      ) : null}
    </DetailShell>
  );
}
