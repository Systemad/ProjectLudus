export const PAGE_GUTTER = 16;

export const GAME_RAIL_GAP = 10;
export const GAME_RAIL_VISIBLE_CARD_COUNT = 3.2;

export function getGameRailCardWidth(
  viewportWidth: number,
  visibleCardCount = GAME_RAIL_VISIBLE_CARD_COUNT,
) {
  const availableWidth = viewportWidth - PAGE_GUTTER * 2;
  return Math.floor((availableWidth - GAME_RAIL_GAP * (visibleCardCount - 1)) / visibleCardCount);
}
