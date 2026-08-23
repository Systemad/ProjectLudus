import type { InstantSearchStatus } from "instantsearch.js";

export type SearchContentStatus = "loading" | "error" | "empty" | "ready";

type SearchSnapshot = {
  status: InstantSearchStatus;
  error: unknown;
  hasResults: boolean;
};

export const SEARCH_STATE_COPY = {
  errorMessage: "The search service could not be reached.",
  errorTitle: "Search failed",
  emptyMessage: "Try another title or adjust your filters.",
  emptyTitle: "No results found",
} as const;

export function getSearchContentStatus({
  status,
  error,
  hasResults,
}: SearchSnapshot): SearchContentStatus {
  if ((status === "loading" || status === "stalled") && !hasResults) return "loading";
  if (error) return "error";
  if (!hasResults) return "empty";
  return "ready";
}
