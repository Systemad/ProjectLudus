import { Link } from "@tanstack/react-router";
import { Flex, Heading, SimpleGrid, Text, VStack } from "ui";
import { getGameById, trendingGameIds } from "../../data/games";
import { GameCard } from "../GameCard";

const linkStyle = { color: "inherit", textDecoration: "none" };

export function TrendingSection() {
    const trendingGames = trendingGameIds.map((id) => getGameById(id)).filter((g) => g != null);

    return (
        <VStack align="stretch" gap="8">
            <Flex
                justify="space-between"
                align={{ base: "start", md: "end" }}
                gap="4"
                direction={{ base: "column", md: "row" }}
            >
                <VStack align="stretch" gap="2">
                    <Text
                        color="colorScheme.solid"
                        textTransform="uppercase"
                        letterSpacing="widest"
                        fontSize="xs"
                        fontWeight="bold"
                    >
                        Most played
                    </Text>
                    <Heading fontFamily="heading" fontSize={{ base: "3xl", md: "4xl" }}>
                        Trending Now
                    </Heading>
                </VStack>
                <Link to="/search" style={linkStyle}>
                    <Text
                        color="fg.muted"
                        fontWeight="bold"
                        textTransform="uppercase"
                        letterSpacing="widest"
                        _hover={{ color: "colorScheme.solid" }}
                    >
                        View all
                    </Text>
                </Link>
            </Flex>

            <SimpleGrid columns={{ base: 2, sm: 2, xl: 4 }} gap={{ base: 4, md: 8 }}>
                {trendingGames.map((game) => (
                    <GameCard key={game.id} game={game} />
                ))}
            </SimpleGrid>
        </VStack>
    );
}
