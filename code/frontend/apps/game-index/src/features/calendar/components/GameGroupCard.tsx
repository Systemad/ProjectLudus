import { useState } from "react";
import type { GameDto } from "@src/gen/catalogApi";
import { GameRow } from "./GameRow";
import { Box, Collapse, Text, VStack } from "ui";

export function GameGroupCard({
    title,
    games,
    emptyLabel,
    isPlaceholder = false,
}: {
    title: string;
    games: GameDto[];
    emptyLabel: string;
    isPlaceholder?: boolean;
}) {
    const [expanded, setExpanded] = useState(false);
    const hasMore = games.length > 4;
    const groupId = `game-group-${title.replace(/\s+/g, "-")}`;

    return (
        <Box rounded="xl" p={{ base: "3", md: "4" }} bg="bg.panel" h="full">
            <Text fontWeight="semibold" fontSize="lg" mb="2">
                {title}
            </Text>
            {games.length === 0 ? (
                <VStack py="6" color="fg.muted" gap="1">
                    <Text fontSize="2xl">:(</Text>
                    <Text fontSize="sm">{emptyLabel}</Text>
                </VStack>
            ) : (
                <VStack gap="2" align="stretch">
                    {games.slice(0, 4).map((game) => (
                        <GameRow key={game.id} game={game} isPlaceholder={isPlaceholder} />
                    ))}
                    <Collapse open={expanded} id={groupId}>
                        <VStack gap="2" align="stretch">
                            {games.slice(4).map((game) => (
                                <GameRow key={game.id} game={game} isPlaceholder={isPlaceholder} />
                            ))}
                        </VStack>
                    </Collapse>
                    {hasMore && (
                        <Box
                            as="button"
                            type="button"
                            onClick={() => setExpanded((value) => !value)}
                            aria-expanded={expanded}
                            aria-controls={groupId}
                            mt="2"
                            py="2"
                            w="full"
                            textAlign="center"
                            fontSize="sm"
                            color="fg.muted"
                            bg="bg.subtle"
                            rounded="lg"
                            _hover={{ bg: "rgba(255,255,255,0.08)" }}
                        >
                            {expanded ? "Show less" : "Expand all"}
                        </Box>
                    )}
                </VStack>
            )}
        </Box>
    );
}
