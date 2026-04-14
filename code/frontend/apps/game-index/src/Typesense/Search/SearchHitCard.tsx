import type { GameSearchHit } from "./searchUtils";
import { GameHitCard } from "./GameHitCard";

type SearchHitCardProps = {
    hit: GameSearchHit;
};

export function SearchHitCard({ hit }: SearchHitCardProps) {
    return <GameHitCard hit={hit} />;
}
