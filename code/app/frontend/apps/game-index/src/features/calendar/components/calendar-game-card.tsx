import { useState } from "react";
import type { GameBrowseDto } from "@src/gen/catalogApi";
import { Text } from "@astryxdesign/core/Text";
import { DataCard } from "@src/components/data-card";
import { VStack } from "@astryxdesign/core/VStack";
import { GameRow } from "./calendar-game-row";
import { Button } from "@astryxdesign/core/Button";

export function GameGroupCard({
    title,
    games,
    emptyLabel,
    isPlaceholder = false,
}: {
    title: string;
    games: GameBrowseDto[];
    emptyLabel: string;
    isPlaceholder?: boolean;
}) {
    const [expanded, setExpanded] = useState(false);
    const hasMore = games.length > 4;
    const groupId = `game-group-${title.replace(/\s+/g, "-")}`;

    return (
        <DataCard style={{height: "100%"}}>
            <Text weight="semibold" style={{fontSize: "1.125rem", marginBottom: "0.5rem"}}>
                {title}
            </Text>
            {games.length === 0 ? (
                <VStack gap={1} style={{paddingTop: "1.5rem", paddingBottom: "1.5rem"}}>
                    <Text style={{fontSize: "1.5rem"}}>:(</Text>
                    <Text style={{fontSize: "0.875rem"}}>{emptyLabel}</Text>
                </VStack>
            ) : (
                        <VStack gap={2} hAlign="stretch">
                    {games.slice(0, 4).map((game) => (
                        <GameRow key={game.id} game={game} isPlaceholder={isPlaceholder} />
                    ))}
                    <div
                        id={groupId}
                        style={{
                            overflow: "hidden",
                            maxHeight: expanded ? "1000px" : "0",
                            transition: "max-height 0.3s ease",
                        }}
                    >
                <VStack gap={2} hAlign="stretch">
                            {games.slice(4).map((game) => (
                                <GameRow key={game.id} game={game} isPlaceholder={isPlaceholder} />
                            ))}
                        </VStack>
                    </div>
                    {hasMore && (
                    <Button
                        variant="ghost"
                        size="sm"
                        label={expanded ? "Show less" : "Expand all"}
                        onClick={() => setExpanded((value) => !value)}
                        aria-expanded={expanded}
                        aria-controls={groupId}
                        style={{
                            marginTop: "0.5rem",
                            width: "100%",
                        }}
                    />
                    )}
                </VStack>
            )}
        </DataCard>
    );
}
