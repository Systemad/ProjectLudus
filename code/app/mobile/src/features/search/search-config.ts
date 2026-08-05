import type { FacetDefinition } from "./search-types";

export const gameFacets: FacetDefinition[] = [
  { attribute: "game_type", label: "Type" },
  { attribute: "genres", label: "Genres" },
  { attribute: "themes", label: "Themes" },
  { attribute: "game_modes", label: "Modes" },
  { attribute: "multiplayer_modes", label: "Multiplayer" },
  { attribute: "player_perspectives", label: "Perspective" },
];
