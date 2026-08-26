import { Column, FlowRow } from "@expo/ui/jetpack-compose";
import {
  fillMaxSize,
  fillMaxWidth,
  padding,
  verticalScroll,
} from "@expo/ui/jetpack-compose/modifiers";
import { useWindowDimensions } from "react-native";

import { GRID_COLUMN_GAP, GRID_PAGE_PADDING, GRID_ROW_GAP, PAGE_GUTTER } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { getGameDetailHref } from "@/utils/game-routes";

import type { GameSearchHit } from "../search-types";

export function SearchResults({
  items,
  bottomInset,
}: {
  items: GameSearchHit[];
  bottomInset: number;
}) {
  const { width } = useWindowDimensions();
  const cardWidth = Math.floor((width - PAGE_GUTTER * 2 - GRID_COLUMN_GAP) / 2);

  return (
    <Column
      modifiers={[
        fillMaxSize(),
        verticalScroll(),
        padding(PAGE_GUTTER, 4, PAGE_GUTTER, bottomInset + GRID_PAGE_PADDING),
      ]}
    >
      <FlowRow
        horizontalArrangement={{ spacedBy: GRID_COLUMN_GAP }}
        verticalArrangement={{ spacedBy: GRID_ROW_GAP }}
        modifiers={[fillMaxWidth()]}
      >
        {items.map((item) => (
          <GameCard
            key={item.objectID}
            title={item.name ?? "Untitled"}
            metadata={`Game · ${String(item.release_year ?? "Release date unknown")}`}
            imageUrl={item.cover_url ? getIgdbImageUrl(item.cover_url, "cover_big") : undefined}
            href={getGameDetailHref(item.id)}
            variant="grid"
            cardWidth={cardWidth}
          />
        ))}
      </FlowRow>
    </Column>
  );
}
