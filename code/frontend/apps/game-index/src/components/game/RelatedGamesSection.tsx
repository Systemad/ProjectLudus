"use client";

import { Box, Button, HStack, Text, Wrap } from "ui";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";
import GameCard from "@src/components/game/RelatedGameImageCard";

type RelatedGame = {
    id: number;
    name: string;
    imageUrl?: string;
};

type Props = {
    games: RelatedGame[];
};

export default function RelatedGamesSection({ games }: Props) {
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

            {games.length > 0 ? (
                <Wrap gap={4}>
                    {games.slice(0, 6).map((game) => (
                        <Box
                            key={game.id}
                            flexBasis={{
                                base: "calc(50% - 0.5rem)",
                                md: "calc(33.333% - 0.75rem)",
                                lg: "calc(20% - 0.8rem)",
                            }}
                            flexGrow={1}
                            minW={{ base: "9rem", sm: "10rem" }}
                            maxW={{ lg: "13rem" }}
                        >
                            <GameCard name={game.name} imageUrl={game.imageUrl} />
                        </Box>
                    ))}
                </Wrap>
            ) : (
                <Text color="fg.subtle">No related games available.</Text>
            )}
        </Box>
    );
}
