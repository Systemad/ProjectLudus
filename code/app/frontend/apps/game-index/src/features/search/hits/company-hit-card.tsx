import { Text } from "@astryxdesign/core/Text";
import { DataCard } from "@src/components/data-card";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import type { CompanySearchHit } from "../utils/hits";
import { getCompanyStatusLabel } from "../utils/search-utils";

type CompanyHitCardProps = {
    hit: CompanySearchHit;
};

export function CompanyHitCard({ hit }: CompanyHitCardProps) {
    const imageUrl = getIGDBImageUrl(hit.logo_url, "logo_med");

    return (
        <DataCard width="100%">
            <div style={{ aspectRatio: "3/2", overflow: "hidden", borderRadius: "var(--radius-lg)", background: "var(--bg-subtle)" }}>
                {imageUrl ? (
                    <img
                        src={imageUrl}
                        alt={hit.name ? `${hit.name} logo` : "Company logo"}
                        style={{
                            width: "100%",
                            height: "100%",
                            objectFit: "contain",
                            padding: "0.25rem",
                        }}
                    />
                ) : (
                    <div style={{ width: "100%", height: "100%", display: "flex", alignItems: "center", justifyContent: "center" }}>
                        <Text color="secondary" style={{fontSize: "0.875rem"}}>
                            No logo available
                        </Text>
                    </div>
                )}
            </div>

            <div style={{ display: "flex", flexDirection: "column", gap: "0.25rem" }}>
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
                    {hit.name ?? "Unknown company"}
                </Text>

                <Text style={{fontSize: "0.875rem", color: "var(--fg-tertiary)"}}>
                    Status: {getCompanyStatusLabel(hit.status)}
                </Text>

                <Text weight="semibold" style={{fontSize: "0.875rem"}}>
                    Developed: {hit.games_developed_count ?? 0}
                </Text>

                <Text weight="semibold" style={{fontSize: "0.875rem"}}>
                    Published: {hit.games_published_count ?? 0}
                </Text>
            </div>
        </DataCard>
    );
}
