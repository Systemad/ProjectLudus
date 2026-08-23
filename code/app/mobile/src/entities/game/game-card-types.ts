import type { Href } from "expo-router";

import type { GameCardData } from "./game-card-data";

export type GameCardVariant = "grid" | "rail" | "cover";

export type GameCardProps = GameCardData & {
  variant: GameCardVariant;
  href: Href;
  cardWidth?: number;
};
