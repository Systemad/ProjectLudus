import { useQuery } from "@tanstack/react-query";
import { Text, View } from "react-native";
import type { Href } from "expo-router";

import { GameCarousel } from "@/entities/game/game-carousel";
import { eventsGetByIdQueryOptions } from "@/gen/hooks/EventsHooks/index";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { DetailShell, FactGroup, detailStyles } from "@/shared/ui/detail-shell";
import { formatShortDate } from "@/utils/date";

const getEventGameHref = (game: { id: string | number }) =>
  ({
    pathname: "/games/[slug]",
    params: { slug: String(game.id) },
  }) satisfies Href;

export function EventDetail({ slug }: { slug: string }) {
  const colors = useAppTheme();
  const query = useQuery(eventsGetByIdQueryOptions({ path: { id: String(slug) } }));
  const event = query.data?.event;
  const status = getContentStateStatus(query.isLoading, query.isError, !event);

  if (!event || status !== "ready") {
    return (
      <ContentState
        status={status}
        fullScreen
        loading={{ label: "Loading event…" }}
        error={{ onRetry: () => void query.refetch() }}
        empty={{ title: "Event not found", message: "The API did not return this event." }}
      />
    );
  }

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
