import type { Href } from "expo-router";

export function getGameDetailHref(gameId: string | number) {
  return {
    pathname: "/games/[slug]",
    params: { slug: String(gameId) },
  } satisfies Href;
}
