import { Link } from "@tanstack/react-router";
import { AspectRatio, Flex, Heading, HStack, Image, StarIcon, Text, VStack } from "ui";

export type GameCardGame = {
    id: string;
    image: string;
    title: string;
    category: string;
    subtitle: string;
    rating: number;
};

type GameCardProps = {
    game: GameCardGame;
    showSubtitle?: boolean;
};

const linkStyle = { color: "inherit", textDecoration: "none" };

export function GameCard({ game, showSubtitle = true }: GameCardProps) {
    return (
        <Link to="/games/$gameId" params={{ gameId: game.id }} style={linkStyle}>
            <VStack align="stretch" gap="4">
                <AspectRatio ratio={3 / 4} rounded="2xl" overflow="hidden" bg="bg.panel">
                    <Image
                        src={game.image}
                        alt={game.title}
                        h="full"
                        w="full"
                        objectFit="cover"
                        transitionDuration="slower"
                        transitionProperty="transform"
                        _hover={{ transform: "scale(1.06)" }}
                    />
                </AspectRatio>
                <VStack align="stretch" gap="2">
                    <Flex justify="space-between" align="center" gap="2" minW="0">
                        <Text
                            flex="1"
                            minW="0"
                            fontSize="xs"
                            textTransform="uppercase"
                            letterSpacing="widest"
                            color="fg.subtle"
                            fontWeight="bold"
                            whiteSpace="nowrap"
                            overflow="hidden"
                            textOverflow="ellipsis"
                        >
                            {game.category}
                        </Text>
                        <HStack gap="0.5" color="colorScheme.solid" flexShrink={0}>
                            {Array.from({ length: 5 }).map((_, i) => (
                                <StarIcon
                                    key={`${game.id}-star-${i + 1}`}
                                    boxSize="3"
                                    fill={i < Math.round(game.rating) ? "currentColor" : "none"}
                                />
                            ))}
                        </HStack>
                    </Flex>
                    <Heading as="h3" size="md" fontFamily="heading">
                        {game.title}
                    </Heading>
                    {showSubtitle ? <Text color="fg.muted">{game.subtitle}</Text> : null}
                </VStack>
            </VStack>
        </Link>
    );
}
