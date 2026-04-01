import {
    AspectRatio,
    Badge,
    Bleed,
    Box,
    Grid,
    Heading,
    HStack,
    Image,
    StarIcon,
    Text,
    VStack,
    Wrap,
} from "ui";
import type { Game } from "../../data/games";

type Props = {
    game: Game;
};

export default function Hero({ game }: Props) {
    return (
        <Bleed inline="full" blockStart="xl">
            <Box
                position="relative"
                h={{ base: "30vh", md: "55vh" }}
                overflow="hidden"
                mb={{ base: 8, md: 12 }}
            >
                <Image
                    src={game.heroImage}
                    alt={game.title}
                    position="absolute"
                    inset={0}
                    w="full"
                    h="full"
                    objectFit="cover"
                    filter="brightness(0.5)"
                />

                <Box
                    position="absolute"
                    inset={0}
                    bgGradient="linear(to-t, #0a0b0d 0%, transparent 60%)"
                />

                <Box maxW="6xl" mx="auto" px={{ base: 4, md: 6 }} h="full" position="relative">
                    <Grid
                        templateColumns={{ base: "1fr", md: "240px 1fr" }}
                        gap={{ base: 6, md: 8 }}
                        position="absolute"
                        top={{ base: "10%", md: "auto" }}
                        bottom={{ base: "10%", md: "10%" }}
                        left={0}
                        right={0}
                        w="full"
                        px={{ base: 4, md: 0 }}
                        alignItems={{ base: "center", md: "end" }}
                    >
                        <Box
                            display="block"
                            rounded="xl"
                            overflow="hidden"
                            boxShadow="lg"
                            w={{ base: "120px", md: "full" }}
                            mx={{ base: "auto", md: "0" }}
                            zIndex={2}
                        >
                            <AspectRatio ratio={3 / 4}>
                                <Image
                                    src={game.coverImage}
                                    objectFit="cover"
                                    alt="Cover Art"
                                    rounded="2xl"
                                />
                            </AspectRatio>
                        </Box>

                        <VStack align={{ base: "center", md: "start" }} gap={2} zIndex={2}>
                            <HStack gap={3}>
                                <Wrap gap="sm">
                                    {game.genres?.slice(0, 2).map((g) => (
                                        <Badge key={g} color="white" px={3} py={1} rounded="md">
                                            {g.toUpperCase()}
                                        </Badge>
                                    ))}
                                </Wrap>

                                <HStack gap={1} color="yellow.400">
                                    <StarIcon boxSize="4" />
                                    <Text fontWeight="bold" fontSize="lg">
                                        {game.rating.toFixed(1)}
                                    </Text>
                                </HStack>
                            </HStack>

                            <Heading
                                fontSize={{ base: "3xl", md: "7xl" }}
                                textAlign={{ base: "center", md: "left" }}
                                fontWeight="900"
                                lineHeight="1"
                                textTransform="uppercase"
                            >
                                {game.title}
                            </Heading>
                        </VStack>
                    </Grid>
                </Box>
            </Box>
        </Bleed>
    );
}
