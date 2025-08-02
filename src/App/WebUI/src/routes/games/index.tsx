import { MagnifyingGlassIcon } from "@phosphor-icons/react";
import {
    createFileRoute,
    useNavigate,
    useSearch,
} from "@tanstack/react-router";
import {
    Accordion,
    AccordionItem,
    Checkbox,
    CheckboxGroup,
    Flex,
    GridItem,
    Input,
    InputGroup,
    InputLeftElement,
    ScrollArea,
    SimpleGrid,
    VStack,
} from "@yamada-ui/react";
import { useMemo, useState } from "react";
import { FilterAccordion } from "~/features/games/components/Accordion/FilterAccordionItem";
import { z } from "zod";

import {
    publicGamesGetGamesByParametersEndpointHook,
    usePublicGamesGetFiltersEndpointHook,
} from "~/gen";
import { useDebouncedState } from "@mantine/hooks";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";

const commaSeparatedNumberArray = z.preprocess((val) => {
    if (typeof val === "string") {
        return val
            .split(",")
            .map((s) => s.trim())
            .filter(Boolean)
            .map(Number);
    }
    return val;
}, z.array(z.number()));

const gameSearchSchema = z.object({
    page: z.coerce.number().catch(1),
    query: z.coerce.string().optional().catch(""),
    genres: commaSeparatedNumberArray.optional(),
    platforms: commaSeparatedNumberArray.optional(),
    gameModes: commaSeparatedNumberArray.optional(),
    themes: commaSeparatedNumberArray.optional(),
    gameType: commaSeparatedNumberArray.optional(),
    playerPerspectives: commaSeparatedNumberArray.optional(),
});

type GameSearch = z.infer<typeof gameSearchSchema>;

export const Route = createFileRoute("/games/")({
    component: RouteComponent,
    validateSearch: gameSearchSchema,
    loaderDeps: ({ search }) => ({ ...search }),

    loader: ({
        context: { queryClient },
        deps: {
            page,
            query,
            genres,
            platforms,
            gameModes,
            themes,
            gameType,
            playerPerspectives,
        },
    }) => {
        return queryClient.ensureQueryData({
            queryKey: [
                "games",
                "search",
                page,
                query,
                genres,
                platforms,
                gameModes,
                themes,
                gameType,
                playerPerspectives,
            ],
            queryFn: () =>
                publicGamesGetGamesByParametersEndpointHook({
                    pageNumber: page,
                    pageSize: 40,
                    name: query,
                    platforms,
                }),
        });
    },
});

/*
    const { data, isPending, isPlaceholderData, isFetching } =
        usePublicGamesGetTopRatedGamesEndpointHook(
            {
                pageNumber: page,
                pageSize: 40,
            },
            {
                query: {
                    queryKey: ["games", page],
                    placeholderData: keepPreviousData,
                },
            }
        );
*/
function RouteComponent() {
    const { data: filters } = usePublicGamesGetFiltersEndpointHook();

    const {
        page,
        query,
        genres,
        platforms,
        gameModes,
        themes,
        gameType,
        playerPerspectives,
    } = Route.useSearch();
    const navigate = useNavigate({ from: Route.fullPath });

    const updateFilters = (
        name: keyof GameSearch,
        value: number[] | undefined
    ) => {
        navigate({
            search: (prev) => ({
                ...prev,
                [name]: value ? value.join(",") : undefined,
                page: 1,
            }),
            replace: true,
        });
    };

    const data = Route.useLoaderData();
    return (
        <>
            <Flex direction="column" w="full">
                {/* Top Bar */}
                <Flex
                    borderRadius={"xl"}
                    w="full"
                    bg={["blackAlpha.50", "whiteAlpha.100"]}
                    h="5xs"
                    align="center"
                    justify="center"
                >
                    <Input
                        value={query}
                        onChange={(e) => {
                            updateFilters("query", e.target.value);
                        }}
                    />
                    {/* Top bar content here */}
                </Flex>

                {/* Main Content Split */}
                <Flex
                    borderRadius={"xl"}
                    gap="md"
                    mt="4"
                    w="full"
                    minH="md"
                    h="full"
                >
                    {/* Left Side: 1/4 width */}
                    <Flex
                        borderRadius={"xl"}
                        flex="1"
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                        p="4"
                    >
                        <Accordion
                            defaultIndex={[0, 1, 2]}
                            multiple={true}
                            variant="card"
                        >
                            {filters && (
                                <>
                                    <AccordionItem rounded="xl" label={"title"}>
                                        <ScrollArea
                                            h="2xs"
                                            innerProps={{
                                                as: VStack,
                                                gap: "md",
                                            }}
                                        >
                                            {filters && (
                                                <>
                                                    <CheckboxGroup
                                                        value={platforms?.map(
                                                            (id) => id
                                                        )}
                                                        onChange={(e) => {
                                                            updateFilters(
                                                                "platforms",
                                                                e
                                                            );
                                                        }}
                                                    >
                                                        {filters.platforms.map(
                                                            (filter) => (
                                                                <Checkbox
                                                                    key={
                                                                        filter.id
                                                                    }
                                                                    value={
                                                                        filter.id
                                                                    }
                                                                >
                                                                    {
                                                                        filter.name
                                                                    }
                                                                </Checkbox>
                                                            )
                                                        )}
                                                    </CheckboxGroup>
                                                </>
                                            )}
                                        </ScrollArea>
                                    </AccordionItem>
                                </>
                            )}

                            <AccordionItem rounded="xl" label="Platforms">
                                <InputGroup mt="xs">
                                    <InputLeftElement>
                                        <MagnifyingGlassIcon />
                                    </InputLeftElement>
                                    <Input
                                        borderWidth={"thin"}
                                        variant={"filled"}
                                        rounded="xl"
                                        placeholder="Search"
                                    />
                                </InputGroup>
                                <ScrollArea
                                    h="2xs"
                                    innerProps={{ as: VStack, gap: "md" }}
                                ></ScrollArea>
                            </AccordionItem>

                            <AccordionItem
                                rounded="xl"
                                label="Game Launchers"
                            ></AccordionItem>

                            <AccordionItem
                                rounded="xl"
                                label="Release dates"
                            ></AccordionItem>
                        </Accordion>
                    </Flex>
                    {/* Right Side: 3/4 width */}
                    <Flex
                        borderRadius={"xl"}
                        flex="4"
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                        p="4"
                    >
                        {data && (
                            <Flex
                                direction={"column"}
                                alignItems={"center"}
                                gap={"xl"}
                            >
                                <Flex
                                    justifyContent="flex-start"
                                    w="full"
                                ></Flex>
                                <SimpleGrid
                                    columns={{
                                        base: 1,
                                        sm: 2,
                                        md: 2,
                                        lg: 3,
                                        xl: 5,
                                    }}
                                    gap="lg"
                                >
                                    {data?.items.map((item) => (
                                        <GridItem key={item.id}>
                                            <HoverGameCard item={item} />
                                        </GridItem>
                                    ))}
                                </SimpleGrid>
                            </Flex>
                        )}
                    </Flex>
                </Flex>
            </Flex>
        </>
    );
}
