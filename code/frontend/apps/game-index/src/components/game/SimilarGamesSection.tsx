import { Box, Button, Heading, HStack, Image, SimpleGrid, StarIcon, Text, VStack } from "ui";
import type { Game } from "../../data/games";

type SimilarGamesSectionProps = {
    games: Game[];
};

export default function SimilarGamesSection({ games }: SimilarGamesSectionProps) {
    return (
        <Box mt={20}>
            <HStack justify="space-between" mb={6}>
                <VStack align="start" gap={0}>
                    <Heading size="lg">SIMILAR GAMES</Heading>
                    <Text color="fg.subtle">
                        If you enjoyed Hollow Knight, you might like these
                    </Text>
                </VStack>
                <Button variant="ghost" colorScheme="gray" color="fg.base" fontSize="xs">
                    VIEW ALL
                </Button>
            </HStack>
            <SimpleGrid columns={{ base: 1, sm: 2, md: 4 }} gap={6}>
                {games.slice(0, 4).map((game) => (
                    <Box
                        key={game.id}
                        position="relative"
                        rounded="2xl"
                        overflow="hidden"
                        h="280px"
                        role="group"
                    >
                        <Image
                            src={game.coverImage}
                            w="full"
                            h="full"
                            objectFit="cover"
                            transition="transform 0.3s"
                            _groupHover={{ transform: "scale(1.1)" }}
                        />
                        <Box
                            position="absolute"
                            inset={0}
                            bgGradient="linear(to-t, black 0%, transparent 70%)"
                        />
                        <VStack position="absolute" bottom={4} left={4} align="start" gap={0}>
                            <Text fontWeight="bold" fontSize="md">
                                {game.title}
                            </Text>
                            <HStack gap={1} color="yellow.500" fontSize="xs">
                                <StarIcon />
                                <Text color="fg.base" fontWeight="bold">
                                    {game.rating.toFixed(1)}
                                </Text>
                            </HStack>
                        </VStack>
                    </Box>
                ))}
            </SimpleGrid>
        </Box>
    );
}
