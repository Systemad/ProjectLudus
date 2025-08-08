import { MagnifyingGlassIcon } from "@phosphor-icons/react";
import { createFileRoute, useNavigate } from "@tanstack/react-router";
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
import { useEffect, useMemo, useState } from "react";
import { FilterAccordion } from "~/features/games/components/Accordion/FilterAccordionItem";
import { z } from "zod";
import {
    publicGamesGetGamesByParametersEndpointHook,
    usePublicGamesGetFiltersEndpointHook,
} from "~/gen";
import { useDebouncedState, useDebouncedValue } from "@mantine/hooks";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";

const gameSearchSchema = z.object({
    page: z.coerce.number().optional().catch(1),
    query: z.string().optional().catch(""),
    genres: z.array(z.coerce.number()).optional(),
    platforms: z.array(z.coerce.number()).optional(),
});

type GameSearch = z.infer<typeof gameSearchSchema>;

export const Route = createFileRoute("/games/")({
    component: RouteComponent,
    validateSearch: gameSearchSchema,
    loaderDeps: ({ search }) => ({ ...search }),

    loader: ({
        context: { queryClient },
        deps: { page, query, genres, platforms },
    }) => {
        return queryClient.ensureQueryData({
            queryKey: ["games", "search", page, query, genres, platforms],
            queryFn: () =>
                publicGamesGetGamesByParametersEndpointHook({
                    params: {
                        pageNumber: page,
                        pageSize: 40,
                        name: query,
                        genres: genres,
                        platforms: platforms,
                    },
                }),
        });
    },
});

function RouteComponent() {
    const { data: filters } = usePublicGamesGetFiltersEndpointHook();

    const { page, query, genres, platforms } = Route.useSearch();
    const navigate = useNavigate({ from: Route.fullPath });

    const updateFilters = (name: keyof GameSearch, value: unknown) => {
        navigate({
            search: (prev) => ({ ...prev, [name]: value, page: 1 }),
            replace: true,
        });
    };

    const [value, setValue] = useState("");
    const [debounced] = useDebouncedValue(value, 200);

    useEffect(() => {
        updateFilters("query", debounced);
    }, [debounced]);
    /*

                              <FilterAccordion
                                        title={"Genres"}
                                        items={filters.genres}
                                        selected={genres ?? []}
                                        onChange={(e) =>
                                            updateFilters2("genres", e)
                                        }
                                    />

                                    
                   <Input
                        value={query}
                        onChange={(e) => {
                            updateSearchQuery("query", e.target.value);
                        }}
                    />
    */
    const data = Route.useLoaderData();
    return (
        <>
            <Flex direction="column" w="full">
                {/* Top Bar */}
                <Flex
                    px="md"
                    borderRadius={"xl"}
                    w="md"
                    bg={["blackAlpha.50", "whiteAlpha.100"]}
                    h="5xs"
                    align="center"
                    justify="center"
                >
                    <Input
                        value={query}
                        onChange={(e) => {
                            //updateFilters("query", e.target.value);
                            setValue(e.currentTarget.value);
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
                                    <FilterAccordion
                                        title={"Platforms"}
                                        items={filters.platforms}
                                        selected={platforms ?? []}
                                        onChange={(e) =>
                                            updateFilters("platforms", e)
                                        }
                                    />

                                    <FilterAccordion
                                        title={"Genres"}
                                        items={filters.genres}
                                        selected={genres ?? []}
                                        onChange={(e) =>
                                            updateFilters("genres", e)
                                        }
                                    />
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
