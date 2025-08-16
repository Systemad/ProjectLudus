import { MagnifyingGlassIcon } from "@phosphor-icons/react";
import {
    createFileRoute,
    useNavigate,
    stripSearchParams,
} from "@tanstack/react-router";
import {
    Accordion,
    Button,
    Flex,
    GridItem,
    Input,
    InputGroup,
    InputLeftElement,
    SimpleGrid,
} from "@yamada-ui/react";
import { useState } from "react";
import { FilterAccordion } from "~/features/games/components/Accordion/FilterAccordionItem";
import { z } from "zod";
import {
    publicGamesGetGamesByParametersEndpointHook,
    usePublicGamesGetFiltersEndpointHook,
} from "~/gen";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";

const defaultValues = {
    page: 1,
    query: "",
    genres: [],
    platforms: [],
};

const gameSearchSchema = z.object({
    page: z.coerce.number().optional().catch(1).default(1),
    query: z.string().optional().catch("").default(""),
    genres: z.array(z.coerce.number()).optional().default([]),
    platforms: z.array(z.coerce.number()).optional().default([]),
});

type GameSearch = z.infer<typeof gameSearchSchema>;

export const Route = createFileRoute("/games/")({
    component: RouteComponent,
    validateSearch: gameSearchSchema,
    search: {
        middlewares: [stripSearchParams(defaultValues)],
    },
    loaderDeps: ({ search }) => ({ ...search }),

    loader: ({
        context: { queryClient },
        deps: { page, query, genres, platforms },
    }) => {
        return queryClient.ensureQueryData({
            queryKey: ["games", "search", page, query, genres, platforms],

            queryFn: async () =>
                await publicGamesGetGamesByParametersEndpointHook({
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

    const [value, setValue] = useState<string>(query ?? "");

    const updateFilters = (name: keyof GameSearch, value: unknown) => {
        navigate({
            search: (prev) => ({ ...prev, [name]: value, page: page }),
            replace: true,
        });
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setValue(e.target.value);
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") updateFilters("query", value);
    };

    const handleSearch = () => {
        updateFilters("query", value);
    };

    const data = Route.useLoaderData();
    return (
        <>
            <Flex direction="column" gap="0" w="full">
                {/* Top Bar */}
                <Flex
                    borderRadius={"xl"}
                    w="full"
                    h="5xs"
                    p="0"
                    gap="md"
                    align="center"
                    justify="center"
                >
                    <InputGroup>
                        <InputLeftElement>
                            <MagnifyingGlassIcon />
                        </InputLeftElement>
                        <Input
                            placeholder="Search games"
                            value={value}
                            onKeyDown={handleKeyDown}
                            onChange={handleChange}
                        />
                    </InputGroup>

                    <Button colorScheme={"emerald"} onClick={handleSearch}>
                        Search
                    </Button>

                    {/* Top bar content here */}
                </Flex>

                {/* Main Content Split */}
                <Flex
                    borderRadius={"xl"}
                    gap="md"
                    mt="2"
                    w="full"
                    minH="md"
                    h="full"
                >
                    {/* Left Side: 1/4 width */}
                    <Flex borderRadius={"xl"} flex="1" p="0">
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
                        </Accordion>
                    </Flex>
                    {/* Right Side: 3/4 width */}
                    <Flex
                        borderRadius={"xl"}
                        flex="4"
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                        p="2"
                    >
                        {data && (
                            <Flex direction={"column"} alignItems={"center"}>
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
