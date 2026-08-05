import { useQuery } from "@tanstack/react-query";
import { Text, View } from "react-native";
import type { Href } from "expo-router";

import { GameCarousel } from "@/entities/game/game-carousel";
import { eventsGetByIdQueryOptions } from "@/gen/hooks/EventsHooks";
import { useAppTheme } from "@/hooks/use-app-theme";
import { DetailShell, FactGroup, detailStyles } from "@/shared/ui/detail-shell";
import { EmptyState, ErrorState, LoadingState } from "@/shared/ui/screen-state";
import { formatShortDate } from "@/utils/date";

const getEventGameHref = (game: { id: string | number }) =>
  ({
    pathname: "../games/[slug]",
    params: { slug: String(game.id) },
  }) satisfies Href;

export function EventDetail({ slug }: { slug: string }) {
  const colors = useAppTheme();
  const query = useQuery(eventsGetByIdQueryOptions({ path: { id: String(slug) } }));
  if (query.isLoading) return <LoadingState label="Loading event…" />;
  if (query.isError) return <ErrorState onRetry={() => query.refetch()} />;
  if (!query.data?.event)
    return <EmptyState title="Event not found" message="The API did not return this event." />;

  const event = query.data.event;
  const dates = [event.startTimeUtc, event.endTimeUtc]
    .filter((value): value is string => Boolean(value))
    .map(formatShortDate);
  return (
    <DetailShell
      title={event.name}
      eyebrow="Event"
      summary={event.description ?? "No event description is available yet."}
    >
      <FactGroup title="Dates" values={dates} />
      {event.games.length ? (
        <View style={detailStyles.section}>
          <Text style={[detailStyles.sectionTitle, { color: colors.text }]}>Featured games</Text>
          <GameCarousel games={event.games} getHref={getEventGameHref} />
        </View>
      ) : null}
    </DetailShell>
  );
}
