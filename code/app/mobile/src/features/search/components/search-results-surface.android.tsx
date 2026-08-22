import { useHits, useInstantSearch } from "react-instantsearch-core";
import { Box, FloatingActionButton, Host, Icon } from "@expo/ui/jetpack-compose";
import { align, fillMaxSize, offset } from "@expo/ui/jetpack-compose/modifiers";

import { ContentState } from "@/shared/ui/content-state";
import type { GameSearchHit } from "../search-types";
import { SearchResults } from "./search-results.android";

export function SearchResultsSurface({
  bottomInset,
  onFilterOpen,
}: {
  bottomInset: number;
  onFilterOpen: () => void;
}) {
  const { items } = useHits<GameSearchHit>();
  const { status, error, refresh } = useInstantSearch({ catchError: true });

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
    <Host style={{ flex: 1 }}>
      <Box modifiers={[fillMaxSize()]}>
        <SearchResults items={items} bottomInset={bottomInset} />
        <FloatingActionButton
          onClick={onFilterOpen}
          modifiers={[align("bottomEnd"), offset(-16, -16)]}
        >
          <FloatingActionButton.Icon>
            <Icon source={require("@/assets/icons/filter.xml")} contentDescription="Filter games" />
          </FloatingActionButton.Icon>
        </FloatingActionButton>
      </Box>
    </Host>
  );
}
