import { useHits, useInstantSearch } from "react-instantsearch-core";
import { Box, FloatingActionButton, Host, Icon } from "@expo/ui/jetpack-compose";
import { align, fillMaxSize, offset } from "@expo/ui/jetpack-compose/modifiers";

import { ContentState } from "@/shared/ui/content-state";
import { CONTENT_STATE_MIN_HEIGHT } from "@/config/layout";
import { getSearchContentStatus, SEARCH_STATE_COPY } from "../search-state";
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
  const contentStatus = getSearchContentStatus({
    status,
    error,
    hasResults: items.length > 0,
  });

  if (contentStatus === "loading") {
    return <ContentState status="loading" minHeight={CONTENT_STATE_MIN_HEIGHT} />;
  }

  if (contentStatus === "error") {
    return (
      <ContentState
        status="error"
        error={{
          title: SEARCH_STATE_COPY.errorTitle,
          message: SEARCH_STATE_COPY.errorMessage,
          onRetry: refresh,
        }}
        minHeight={CONTENT_STATE_MIN_HEIGHT}
      />
    );
  }

  if (contentStatus === "empty") {
    return (
      <ContentState
        status="empty"
        empty={{
          title: SEARCH_STATE_COPY.emptyTitle,
          message: SEARCH_STATE_COPY.emptyMessage,
        }}
        minHeight={CONTENT_STATE_MIN_HEIGHT}
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
