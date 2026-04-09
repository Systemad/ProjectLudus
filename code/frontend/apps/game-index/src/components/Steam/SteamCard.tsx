import { Box, Image, Text, VStack, Card } from "ui";
import { Link } from "@tanstack/react-router";
import type { GamesSearch } from "@src/gen/catalogApi/types/GamesSearch";
import { formatReleaseDate } from "@src/utils/formatReleaseDate";

function getCoverImageUrl(coverUrl?: string | null) {
    if (!coverUrl) return "";
    if (coverUrl.startsWith("http")) return coverUrl;
    return `https://images.igdb.com/igdb/image/upload/t_cover_big/${coverUrl}.jpg`;
}

export function SteamCard({ game }: { game: GamesSearch }) {
    return (
        <Link
            to="/games/$gameId"
            params={{ gameId: String(game.id ?? "") }}
            style={{ color: "inherit", textDecoration: "none" }}
        >
            <Card.Root
                as="article"
                variant="elevated"
                rounded="3xl"
                overflow="hidden"
                w="full"
                maxW={{ base: "xs", md: "sm" }}
                _hover={{ transform: "translateY(-1px)", transition: "transform 200ms ease" }}
            >
                <Box position="relative" maxH="80" w="full">
                    <Image
                        src={getCoverImageUrl(game.coverUrl ?? undefined)}
                        alt={game.name ?? "Game cover"}
                        w="full"
                        h="full"
                        objectFit="cover"
                        bg="gray.800"
                    />
                    <Box
                        position="absolute"
                        inset="0"
                        bgGradient="linear(to-t, blackAlpha.700, transparent 40%)"
                    />
                    <VStack
                        position="absolute"
                        bottom="3"
                        left="3"
                        right="3"
                        align="start"
                        gap="1"
                        pointerEvents="none"
                    >
                        <Text
                            fontWeight="black"
                            fontSize={{ base: "md", md: "lg" }}
                            color="white"
                            whiteSpace="nowrap"
                            overflow="hidden"
                            textOverflow="ellipsis"
                        >
                            {game.name ?? "Untitled game"}
                        </Text>
                        <Text color="whiteAlpha.800" fontSize="xs">
                            {formatReleaseDate(game.firstReleaseDate)}
                        </Text>
                    </VStack>
                </Box>
            </Card.Root>
        </Link>
    );
}
