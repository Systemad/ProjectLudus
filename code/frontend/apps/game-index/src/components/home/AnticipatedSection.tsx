import { Badge, Box, Grid, Heading, HStack, Image, SimpleGrid, Text, VStack } from "ui";
import { anticipatedGames } from "../../data/games";

export function AnticipatedSection() {
    return (
        <VStack align="stretch" gap="8">
            <VStack align="stretch" gap="2">
                <Text
                    color="colorScheme.solid"
                    textTransform="uppercase"
                    letterSpacing="widest"
                    fontSize="xs"
                    fontWeight="bold"
                >
                    Upcoming giants
                </Text>
                <Heading fontFamily="heading" fontSize={{ base: "3xl", md: "4xl" }}>
                    Most Anticipated This Year
                </Heading>
            </VStack>

            <SimpleGrid columns={{ base: 1, xl: 2 }} gap="8">
                {anticipatedGames.map((game) => (
                    <Box
                        key={game.id}
                        position="relative"
                        rounded="3xl"
                        overflow="hidden"
                        minH={{ base: "lg", md: "xl" }}
                        borderWidth="1px"
                        borderColor="border.subtle"
                        boxShadow="0 2xl 3xl rgba(0, 0, 0, 0.28)"
                    >
                        <Image
                            src={game.image}
                            alt={game.title}
                            h="full"
                            w="full"
                            objectFit="cover"
                        />
                        <Box
                            position="absolute"
                            inset="0"
                            bgGradient="linear(to-t, rgba(0,0,0,0.92), rgba(0,0,0,0.18))"
                        />
                        <VStack position="absolute" insetX="0" top="0" p="6" align="start" gap="3">
                            <HStack gap="2" flexWrap="wrap">
                                <Badge
                                    rounded="full"
                                    bg="colorScheme.solid"
                                    color="fg.contrast"
                                    px="3"
                                    py="1"
                                >
                                    {game.releaseWindow}
                                </Badge>
                                <Badge
                                    rounded="full"
                                    bg="rgba(0,0,0,0.35)"
                                    color="fg.base"
                                    px="3"
                                    py="1"
                                >
                                    {game.meta[2]}
                                </Badge>
                            </HStack>
                        </VStack>
                        <VStack
                            position="absolute"
                            insetX="0"
                            bottom="0"
                            p={{ base: "6", md: "8" }}
                            align="stretch"
                            gap="6"
                        >
                            <Heading
                                fontFamily="heading"
                                fontSize={{ base: "3xl", md: "5xl" }}
                                lineHeight="1"
                                letterSpacing="tight"
                            >
                                {game.title}
                            </Heading>
                            <Grid
                                templateColumns="repeat(3, minmax(0, 1fr))"
                                gap="4"
                                borderTopWidth="1px"
                                borderColor="border.base"
                                pt="5"
                            >
                                {game.meta.map((item, index) => (
                                    <VStack key={item} align="stretch" gap="1">
                                        <Text
                                            fontSize="xs"
                                            textTransform="uppercase"
                                            letterSpacing="wider"
                                            color="fg.subtle"
                                            fontWeight="bold"
                                        >
                                            {index === 0
                                                ? "Genre"
                                                : index === 1
                                                  ? "Platform"
                                                  : "Signal"}
                                        </Text>
                                        <Text fontSize="sm" fontWeight="semibold">
                                            {item}
                                        </Text>
                                    </VStack>
                                ))}
                            </Grid>
                        </VStack>
                    </Box>
                ))}
            </SimpleGrid>
        </VStack>
    );
}
