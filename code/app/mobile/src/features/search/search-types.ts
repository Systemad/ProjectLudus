import type { Href } from "expo-router";

export type SearchHitFields = {
  objectID: string;
  [key: string]: string | number | string[] | null | undefined;
};

export type GameSearchHit = SearchHitFields & {
  id: string | number;
  name?: string | null;
  cover_url?: string | null;
  release_year?: number | null;
};

export type FacetDefinition = {
  attribute: string;
  label: string;
};

export function getSearchGameHref(id: string | number) {
  return {
    pathname: "/games/[slug]",
    params: { slug: String(id) },
  } satisfies Href;
}
