import { useHits, useInstantSearch } from "react-instantsearch-core";
import { Column, FlowRow } from "@expo/ui/jetpack-compose";
import {
  fillMaxSize,
  fillMaxWidth,
  padding,
  verticalScroll,
} from "@expo/ui/jetpack-compose/modifiers";
import { type Href } from "expo-router";
import { useWindowDimensions } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { ContentState } from "@/shared/ui/content-state";
import type { GameSearchHit } from "../search-types";

const getSearchGameHref = (id: string | number) =>
  ({
    pathname: "/(search)/games/[slug]",
    params: { slug: String(id) },
  }) satisfies Href;

export function SearchResults({ bottomInset }: { bottomInset: number }) {
  const { items } = useHits<GameSearchHit>();
  const { status, error, refresh } = useInstantSearch({ catchError: true });
  const { width } = useWindowDimensions();
  const cardWidth = Math.floor((width - PAGE_GUTTER * 2 - 12) / 2);

  if ((status === "loading" || status === "stalled") && items.length === 0) {
    return <ContentState status="loading" minHeight={240} />;
  }

  if (error) {
    return (
      <ContentState
        status="error"
        error={{
          title: "Search failed",
          message: "The search service could not be reached.",
          onRetry: refresh,
        }}
        minHeight={240}
      />
    );
  }

  if (items.length === 0) {
    return (
      <ContentState
        status="empty"
        empty={{
          title: "No results found",
          message: "Try another title or adjust your filters.",
        }}
        minHeight={240}
      />
    );
  }

  return (
    <Column
      modifiers={[
        fillMaxSize(),
        verticalScroll(),
        padding(PAGE_GUTTER, 4, PAGE_GUTTER, bottomInset + 20),
      ]}
    >
      <FlowRow
        horizontalArrangement={{ spacedBy: 12 }}
        verticalArrangement={{ spacedBy: 12 }}
        modifiers={[fillMaxWidth()]}
      >
        {items.map((item) => (
          <GameCard
            key={item.objectID}
            title={item.name ?? "Untitled"}
            metadata={`Game · ${String(item.release_year ?? "Release date unknown")}`}
            imageUrl={item.cover_url ? getIgdbImageUrl(item.cover_url, "cover_big") : undefined}
            href={getSearchGameHref(item.id)}
            variant="grid"
            cardWidth={cardWidth}
            fillFraction={1}
          />
        ))}
      </FlowRow>
    </Column>
  );
}
