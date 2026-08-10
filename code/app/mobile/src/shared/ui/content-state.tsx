import type { ReactNode } from "react";

import { InlineState } from "@/shared/ui/inline-state";

export type ContentStateStatus = "loading" | "error" | "empty" | "ready";

export type ContentStateLayoutProps = {
  fullScreen?: boolean;
  minHeight?: number;
};

type LoadingContentConfig = {
  label?: string;
};

type ErrorContentConfig = {
  title?: string;
  message?: string;
  onRetry?: () => void;
  retryLabel?: string;
};

type EmptyContentConfig = {
  title?: string;
  message?: string;
};

export type ContentStateProps = ContentStateLayoutProps & {
  status: ContentStateStatus;
  children?: ReactNode;
  loading?: LoadingContentConfig;
  error?: ErrorContentConfig;
  empty?: EmptyContentConfig;
};

export function getContentStateStatus(
  isLoading: boolean,
  isError: boolean,
  isEmpty: boolean,
): ContentStateStatus {
  if (isLoading) return "loading";
  if (isError) return "error";
  if (isEmpty) return "empty";
  return "ready";
}

type LoadingStateProps = ContentStateLayoutProps & LoadingContentConfig;

export function LoadingState({
  label = "Loading…",
  fullScreen = false,
  minHeight = 120,
}: LoadingStateProps) {
  return <InlineState fullScreen={fullScreen} loading message={label} minHeight={minHeight} />;
}

type ErrorStateProps = ContentStateLayoutProps & ErrorContentConfig;

export function ErrorState({
  title = "Couldn’t load this content",
  message = "Please try again.",
  onRetry,
  retryLabel = "Try again",
  fullScreen = false,
  minHeight = 120,
}: ErrorStateProps) {
  return (
    <InlineState
      fullScreen={fullScreen}
      title={title}
      message={message}
      onRetry={onRetry}
      retryLabel={retryLabel}
      minHeight={minHeight}
    />
  );
}

type EmptyStateProps = ContentStateLayoutProps & EmptyContentConfig;

export function EmptyState({
  title = "No results found",
  message = "There is nothing to show yet.",
  fullScreen = false,
  minHeight = 120,
}: EmptyStateProps) {
  return (
    <InlineState fullScreen={fullScreen} title={title} message={message} minHeight={minHeight} />
  );
}

export function ContentState({
  status,
  children,
  fullScreen = false,
  minHeight = 120,
  loading: loadingConfig,
  error: errorConfig,
  empty: emptyConfig,
}: ContentStateProps) {
  if (status === "ready") return children;

  const layout = { fullScreen, minHeight };

  switch (status) {
    case "loading":
      return <LoadingState {...layout} {...loadingConfig} />;
    case "error":
      return <ErrorState {...layout} {...errorConfig} />;
    case "empty":
      return <EmptyState {...layout} {...emptyConfig} />;
  }
}
