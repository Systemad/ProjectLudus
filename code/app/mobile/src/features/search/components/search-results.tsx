import { useHits, useInstantSearch } from "react-instantsearch-core";
import { CONTENT_STATE_MIN_HEIGHT, PAGE_GUTTER } from "@/config/layout";
import { GameGrid } from "@/entities/game/game-grid";
import { getIgdbImageUrl } from "@/entities/game/game-image";
import { ContentState } from "@/shared/ui/content-state";
import { getGameDetailHref } from "@/utils/game-routes";

import { getSearchContentStatus, SEARCH_STATE_COPY } from "../search-state";
import type { GameSearchHit } from "../search-types";

export function SearchResults({ bottomInset }: { bottomInset: number }) {
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
    <GameGrid
      items={items.map((item) => ({
        id: item.objectID,
        title: item.name ?? "Untitled",
        metadata: `Game · ${String(item.release_year ?? "Release date unknown")}`,
        imageUrl: item.cover_url ? getIgdbImageUrl(item.cover_url, "cover_big") : undefined,
        href: getGameDetailHref(item.id),
      }))}
      bottomInset={bottomInset + 20}
      pagePadding={PAGE_GUTTER}
      topPadding={4}
    />
  );
}
