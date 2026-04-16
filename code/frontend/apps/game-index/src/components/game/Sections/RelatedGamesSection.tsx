"use client";

import type { GamesSearchDto } from "@src/gen/catalogApi";
import { Box, Button, HStack, Text, Wrap, For, EmptyState, Gamepad2Icon } from "ui";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";
import { GameCard } from "@src/components/game/GameCard";

type Props = {
    games: GamesSearchDto[];
};

export function RelatedGamesSection({ games }: Props) {
    return (
        <Box>
            <HStack justify="space-between" align="start" mb={4} gap={4} flexWrap="wrap">
                <Box>
                    <Text {...sectionLabelStyle} mb={1}>
                        Related games
                    </Text>
                </Box>
                <Button variant="ghost" colorScheme="gray" size="sm">
                    Explore more games
                </Button>
            </HStack>

            <Wrap gap={4}>
                <For
                    each={games}
                    limit={6}
                    fallback={
                        <EmptyState.Root
                            description="No related games found"
                            indicator={<Gamepad2Icon />}
                        />
                    }
                >
                    {(game) => (
                        <Box
                            key={String(game.id ?? game.name ?? "unknown")}
                            flexBasis={{
                                base: "calc(50% - 0.5rem)",
                                md: "calc(33.333% - 0.75rem)",
                                lg: "calc(20% - 0.8rem)",
                            }}
                            minW={{ base: "9rem", sm: "10rem" }}
                            maxW={{ lg: "13rem" }}
                        >
                            <GameCard game={game} />
                        </Box>
                    )}
                </For>
            </Wrap>
        </Box>
    );
}
