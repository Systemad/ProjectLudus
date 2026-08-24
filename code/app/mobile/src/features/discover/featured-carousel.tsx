import { GameCarousel } from "@/entities/game/game-carousel";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { Href } from "expo-router";

type FeaturedCarouselProps = {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
};

export function FeaturedCarousel({ games, getHref }: FeaturedCarouselProps) {
  return <GameCarousel games={games} getHref={getHref} variant="cover" />;
}
