import { createFileRoute, Link } from "@tanstack/react-router";
import {
    AspectRatio,
    Badge,
    Box,
    Button,
    Checkbox,
    Flex,
    Grid,
    Heading,
    HStack,
    Image,
    Input,
    SearchIcon,
    Select,
    SimpleGrid,
    StarIcon,
    Text,
    VStack,
    Wrap,
} from "ui";
import { useState } from "react";
import { games, searchCollections } from "../data/games";

export const Route = createFileRoute("/search")({ component: SearchPage });

const linkStyle = { color: "inherit", textDecoration: "none" };

const filterGroups = [
    {
        items: searchCollections.genres,
        title: "Genre",
    },
    {
        items: searchCollections.platforms,
        title: "Platform",
    },
    {
        items: searchCollections.availability,
        title: "Availability",
    },
];

function SearchPage() {
    const [sort, setSort] = useState<string>("top-rated");
    const [ascending, setAscending] = useState<boolean>(true);
    return (
        <Box maxW="7xl" mx="auto" pb="16">
            <Grid
                templateColumns={{ base: "1fr", xl: "18rem minmax(0, 1fr)" }}
                gap={{ base: "8", xl: "10" }}
            >
                <VStack
                    as="aside"
                    align="stretch"
                    gap="8"
                    bg="bg.panel"
                    rounded={{ base: "2xl", xl: "3xl" }}
                    borderWidth="1px"
                    borderColor="border.subtle"
                    px={{ base: "5", md: "6" }}
                    py={{ base: "6", md: "8" }}
                    position={{ base: "static", xl: "sticky" }}
                    top="24"
                    h="fit-content"
                >
                    {filterGroups.map((group) => (
                        <VStack key={group.title} align="stretch" gap="4">
                            <Text
                                fontFamily="heading"
                                fontSize="xs"
                                fontWeight="bold"
                                letterSpacing="widest"
                                textTransform="uppercase"
                                color="fg.muted"
                            >
                                {group.title}
                            </Text>
                            <VStack align="stretch" gap="3">
                                {group.items.map((item, index) => (
                                    <Checkbox
                                        key={item}
                                        defaultChecked={index === 0}
                                        colorScheme="yellow"
                                    >
                                        <Text fontSize="sm">{item}</Text>
                                    </Checkbox>
                                ))}
                            </VStack>
                        </VStack>
                    ))}
                </VStack>

                <VStack align="stretch" gap="8">
                    <Flex
                        justify="space-between"
                        gap="4"
                        direction={{ base: "column", md: "row" }}
                        align={{ base: "stretch", md: "center" }}
                    >
                        <Box position="relative" flex="1">
                            <SearchIcon
                                position="absolute"
                                insetInlineStart="4"
                                top="50%"
                                transform="translateY(-50%)"
                                color="fg.subtle"
                            />
                            <Input
                                placeholder="Search games..."
                                rounded="lg"
                                size="lg"
                                borderColor="border.base"
                                bg="bg.panel"
                                h="14"
                            />
                        </Box>
                        <HStack gap="3" flexWrap="wrap" align="center">
                            <Box
                                rounded="lg"
                                borderWidth="1px"
                                borderColor="colorScheme.outline"
                                bg="colorScheme.bg"
                                px="3"
                                py="1"
                                minW="160px"
                            >
                                <Select.Root value={sort} onChange={setSort}>
                                    <Select.Option value="top-rated">Top rated</Select.Option>
                                    <Select.Option value="newest">Newest</Select.Option>
                                    <Select.Option value="most-played">Most played</Select.Option>
                                </Select.Root>
                            </Box>

                            <Button
                                rounded="lg"
                                variant="ghost"
                                size="md"
                                onClick={() => setAscending((v) => !v)}
                                aria-label="Toggle ascending/descending"
                                title={ascending ? "Ascending" : "Descending"}
                            >
                                {ascending ? "▲" : "▼"}
                            </Button>
                        </HStack>
                    </Flex>

                    <SimpleGrid columns={{ base: 1, md: 2, xl: 3 }} gap="6">
                        {games.slice(0, 6).map((game) => (
                            <Box
                                key={game.id}
                                rounded="2xl"
                                bg="bg.panel"
                                borderWidth="1px"
                                borderColor="border.subtle"
                                overflow="hidden"
                            >
                                <AspectRatio ratio={4 / 3}>
                                    <Image
                                        src={game.image}
                                        alt={game.title}
                                        h="full"
                                        w="full"
                                        objectFit="cover"
                                    />
                                </AspectRatio>
                                <VStack align="stretch" gap="4" p="5">
                                    <Flex justify="space-between" gap="3" align="start">
                                        <VStack align="stretch" gap="1">
                                            <Text
                                                color="fg.subtle"
                                                fontSize="xs"
                                                textTransform="uppercase"
                                                letterSpacing="widest"
                                                fontWeight="bold"
                                            >
                                                {game.category}
                                            </Text>
                                            <Heading as="h3" size="md" fontFamily="heading">
                                                {game.title}
                                            </Heading>
                                        </VStack>
                                        <Badge
                                            rounded="full"
                                            px="3"
                                            py="1"
                                            bg="colorScheme.bg"
                                            color="colorScheme.fg"
                                        >
                                            {game.rating.toFixed(1)}
                                        </Badge>
                                    </Flex>

                                    <Wrap gap="2">
                                        {game.themes.slice(0, 2).map((tag) => (
                                            <Badge
                                                key={tag}
                                                rounded="full"
                                                px="3"
                                                py="1"
                                                bg="bg.subtle"
                                                color="fg.base"
                                            >
                                                {tag}
                                            </Badge>
                                        ))}
                                    </Wrap>

                                    <Text color="fg.muted">{game.subtitle}</Text>

                                    <Flex justify="space-between" align="center" gap="3">
                                        <HStack gap="1" color="colorScheme.solid">
                                            {Array.from({ length: 5 }).map((_, index) => (
                                                <StarIcon
                                                    key={`${game.id}-rating-${index}`}
                                                    boxSize="3.5"
                                                    fill={
                                                        index < Math.round(game.rating)
                                                            ? "currentColor"
                                                            : "none"
                                                    }
                                                />
                                            ))}
                                        </HStack>
                                        <Link
                                            to="/games/$gameId"
                                            params={{ gameId: game.id }}
                                            style={linkStyle}
                                        >
                                            <Button
                                                size="sm"
                                                variant="ghost"
                                                color="colorScheme.solid"
                                            >
                                                Open
                                            </Button>
                                        </Link>
                                    </Flex>
                                </VStack>
                            </Box>
                        ))}
                    </SimpleGrid>
                </VStack>
            </Grid>
        </Box>
    );
}
