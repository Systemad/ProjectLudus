import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";

export function uniqueGames(games: readonly GameBrowseDto[]) {
  const seen = new Set<string>();
  return games.filter((game) => {
    if (seen.has(game.id)) return false;
    seen.add(game.id);
    return true;
  });
}
