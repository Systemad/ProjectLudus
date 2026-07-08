"use client";

import type { GameBrowseDto } from "@src/gen/catalogApi";
import { HStack } from "@astryxdesign/core/HStack";
import { Text } from "@astryxdesign/core/Text";
import { GameCard } from "@src/features/game/components/game-card-compact";
import { RouterLinkButton } from "@src/components/router-link";

type Props = {
    games: GameBrowseDto[];
};

export function RelatedGamesSection({ games }: Props) {
    return (
        <div>
            <HStack hAlign="between" gap={4} style={{alignItems: "start", marginBottom: "1rem", flexWrap: "wrap"}}>
                <div>
                    <Text style={{fontSize: "1.25rem", fontWeight: 600, textTransform: "uppercase", letterSpacing: "0.05em", marginBottom: "0.25rem"}}>
                        Related games
                    </Text>
                </div>
                <RouterLinkButton
                    to="/games/search"
                    variant="ghost"
                    size="sm"
                    label="Explore more games"
                />
            </HStack>

            {games.length > 0 ? (
                <HStack wrap="wrap" gap={4}>
                    {games.slice(0, 6).map((game) => (
                        <div
                            key={String(game.id ?? game.name ?? "unknown")}
                            style={{
                                flexBasis: "calc(50% - 0.5rem)",
                                minWidth: "9rem",
                                maxWidth: "13rem",
                            }}
                        >
                            <GameCard game={game} />
                        </div>
                    ))}
                </HStack>
            ) : (
                <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: "1rem", padding: "2rem", color: "var(--fg-muted)" }}>
                    <Text style={{fontSize: "0.875rem"}}>No related games found</Text>
                </div>
            )}
        </div>
    );
}
