import { FileIcon, MagnifyingGlassIcon } from "@phosphor-icons/react";
import {
    createFileRoute,
    useNavigate,
    stripSearchParams,
} from "@tanstack/react-router";
import {
    Accordion,
    Button,
    Center,
    EmptyState,
    EmptyStateDescription,
    EmptyStateIndicator,
    EmptyStateTitle,
    Flex,
    GridItem,
    Input,
    InputGroup,
    InputLeftElement,
    SimpleGrid,
} from "@yamada-ui/react";
import { Suspense, useCallback, useDeferredValue, useState } from "react";
import { FilterAccordion } from "~/features/games/components/Accordion/FilterAccordionItem";
import { z } from "zod";
import {
    publicGamesGetFiltersEndpointSuspenseQueryOptionsHook,
    publicGamesGetGamesByParametersEndpointQueryOptionsHook,
    usePublicGamesGetFiltersEndpointSuspenseHook,
    usePublicGamesGetGamesByParametersEndpointHook,
    type GamePreviewDto,
} from "~/gen";
import { HoverGameCard } from "~/features/games/components/HoverGameCard";
import { useDebouncedValue } from "@mantine/hooks";
import { keepPreviousData, useSuspenseQueries } from "@tanstack/react-query";
import { ErrorBoundary } from "react-error-boundary";
import { FiltersPanel } from "~/features/games/components/Filters/FilterMenu";
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
    loader: async ({ context: { queryClient } }) => {
        queryClient.prefetchQuery(
            publicGamesGetFiltersEndpointSuspenseQueryOptionsHook()
        );
        await queryClient.ensureQueryData(
            publicGamesGetGamesByParametersEndpointQueryOptionsHook({})
        );
    },
});

function RouteComponent() {
    const { data: filters } = usePublicGamesGetFiltersEndpointSuspenseHook();
    const { page, query, genres, platforms } = Route.useSearch();
    const { isPending, isError, error, data, isFetching, isPlaceholderData } =
        usePublicGamesGetGamesByParametersEndpointHook(
            {
                params: {
                    pageNumber: page,
                    pageSize: 40,
                    name: query,
                    genres: genres,
                    platforms: platforms,
                },
            },
            { query: { placeholderData: keepPreviousData } }
        );

    const navigate = useNavigate({ from: Route.fullPath });

    const [value, setValue] = useState<string>(query ?? "");

    const updateSearchParam = useCallback(
        (updates: Partial<z.infer<typeof gameSearchSchema>>) => {
            navigate({
                search: (prev: z.infer<typeof gameSearchSchema>) => ({
                    ...prev,
                    ...updates,
                }),
                replace: true,
            });
        },
        [navigate]
    );

    const updateSearchQueryFilter = useCallback(
        (searchString: string) => {
            updateSearchParam({
                query: searchString as z.infer<
                    typeof gameSearchSchema
                >["query"],
            });
        },
        [navigate]
    );

    const updatePlatformFilter = useCallback(
        (ids: number[]) => {
            updateSearchParam({
                platforms: ids as z.infer<typeof gameSearchSchema>["platforms"],
            });
        },
        [navigate]
    );

    const updateGenreFilter = useCallback(
        (ids: number[]) => {
            updateSearchParam({
                genres: ids as z.infer<typeof gameSearchSchema>["genres"],
            });
        },
        [navigate]
    );

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setValue(e.target.value);
    };
    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") updateSearchQueryFilter(value);
    };
    const handleSearch = () => {
        updateSearchQueryFilter(value);
    };

    //const data = Route.useLoaderData();
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
                                    <FiltersPanel
                                        filters={filters}
                                        selectedPlatforms={platforms ?? []}
                                        selectedGenres={genres ?? []}
                                        onPlatformsChange={updatePlatformFilter}
                                        onGenresChange={updateGenreFilter}
                                    />
                                </>
                            )}
                        </Accordion>
                    </Flex>
                    {/* Right Side: 3/4 width */}
                    <Flex flex="4" p="2">
                        {isPending ? (
                            <div>Loading...</div>
                        ) : isError ? (
                            <div>Error: {error.message}</div>
                        ) : (
                            <div>
                                {data.items.length > 0 ? (
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
                                        <SearchResult items={data.items} />
                                    </SimpleGrid>
                                ) : (
                                    <Center h="full" w="full">
                                        <EmptyState>
                                            <EmptyStateIndicator>
                                                <FileIcon />
                                            </EmptyStateIndicator>
                                            <EmptyStateTitle>
                                                No results found
                                            </EmptyStateTitle>
                                            <EmptyStateDescription>
                                                Try searching for something else
                                            </EmptyStateDescription>
                                        </EmptyState>
                                    </Center>
                                )}
                            </div>
                        )}
                    </Flex>
                </Flex>
            </Flex>
        </>
    );
}

type SearchResultProps = {
    items: GamePreviewDto[];
};

const SearchResult = ({ items }: SearchResultProps) => {
    return (
        <>
            {items.map((item) => (
                <GridItem key={item.id}>
                    <HoverGameCard item={item} height="sm" />
                </GridItem>
            ))}
        </>
    );
};
