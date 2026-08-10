import type { ReactNode } from "react";

import { InlineState } from "@/shared/ui/inline-state";
import { EmptyState, ErrorState, LoadingState } from "@/shared/ui/screen-state";

type CollectionStateProps = {
  isLoading: boolean;
  isError: boolean;
  isEmpty: boolean;
  onRetry: () => void;
  children: ReactNode;
  fullScreen?: boolean;
  minHeight?: number;
  loadingLabel?: string;
  errorTitle?: string;
  errorMessage?: string;
  emptyTitle?: string;
  emptyMessage?: string;
  retryLabel?: string;
};

export function CollectionState({
  isLoading,
  isError,
  isEmpty,
  onRetry,
  children,
  fullScreen = false,
  minHeight = 120,
  loadingLabel = "Loading…",
  errorTitle = "Couldn’t load this content",
  errorMessage = "Please try again.",
  emptyTitle = "No results found",
  emptyMessage = "There is nothing to show yet.",
  retryLabel = "Try again",
}: CollectionStateProps) {
  if (isLoading) {
    return fullScreen ? (
      <LoadingState label={loadingLabel} />
    ) : (
      <InlineState loading message={loadingLabel} minHeight={minHeight} />
    );
  }

  if (isError) {
    return fullScreen ? (
      <ErrorState
        title={errorTitle}
        message={errorMessage}
        onRetry={onRetry}
        retryLabel={retryLabel}
      />
    ) : (
      <InlineState
        title={errorTitle}
        message={errorMessage}
        onRetry={onRetry}
        retryLabel={retryLabel}
        minHeight={minHeight}
      />
    );
  }

  if (isEmpty) {
    return fullScreen ? (
      <EmptyState title={emptyTitle} message={emptyMessage} />
    ) : (
      <InlineState title={emptyTitle} message={emptyMessage} minHeight={minHeight} />
    );
  }

  return children;
}
