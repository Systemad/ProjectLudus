import { Text } from "@astryxdesign/core/Text";
import { DataCard } from "@src/components/data-card";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import type { GameSearchHit } from "../utils/hits";
import { getDevelopersLabel, getReleaseYear } from "../utils/search-utils";

type GameHitCardProps = {
    hit: GameSearchHit;
};

export function GameHitCard({ hit }: GameHitCardProps) {
    const imageUrl = getIGDBImageUrl(hit.cover_url, "cover_big");
    const rating =
        typeof hit.aggregated_rating === "number" ? Math.round(hit.aggregated_rating) : null;
    const votes = hit.aggregated_rating_count ?? 0;
    const releaseYear = getReleaseYear(hit);

    return (
        <DataCard width="100%">
            <div>
                <img
                    src={imageUrl}
                    alt={hit.name ? `${hit.name} cover` : "Game cover"}
                    style={{
                        width: "100%",
                        aspectRatio: "3 / 4",
                        objectFit: "cover",
                    }}
                />
            </div>
            <Text
                as="h3"
                style={{
                    fontSize: "0.875rem",
                    WebkitLineClamp: 2,
                    overflow: "hidden",
                    display: "-webkit-box",
                    WebkitBoxOrient: "vertical",
                    minHeight: "2.5rem",
                    color: "var(--fg-base)",
                }}
            >
                {hit.name ?? "Untitled game"}
            </Text>
            <div style={{ display: "flex", flexDirection: "column", gap: "0.125rem", marginTop: "0.25rem" }}>
                <Text style={{ fontSize: "0.875rem", color: "var(--fg-tertiary)", WebkitLineClamp: 1, overflow: "hidden" }}>
                    {getDevelopersLabel(hit.developers)}
                </Text>
                <Text color="secondary" style={{fontSize: "0.875rem"}}>
                    Released: {releaseYear}
                </Text>
            </div>
            <div style={{ borderRadius: "var(--radius-md)", background: "var(--bg-panel)", padding: "0.25rem 0.5rem", width: "100%" }}>
                <Text style={{ fontSize: "0.875rem", color: "var(--fg-tertiary)", lineHeight: "1.25" }}>
                    Rating:{" "}
                    <Text as="span" style={{fontWeight: 600}}>
                        {rating !== null ? `${rating}/100` : "No rating yet"}
                    </Text>
                    {rating !== null ? ` (${votes} votes)` : ""}
                </Text>
            </div>
        </DataCard>
    );
}
