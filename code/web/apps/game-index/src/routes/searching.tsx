"use client";

import { createFileRoute, stripSearchParams } from "@tanstack/react-router";
import { getApiTagsAllTagsQueryOptionsHook } from "../gen";
import z from "zod/v4";
import { SearchContainer } from "../Components/Search/SearchContainer";

const searchQueryParamsSchema2 = z.object({
    Name: z.string().optional(),
    Genres: z.array(z.string()).optional(),
    Themes: z.array(z.string()).optional(),
    GameModes: z.array(z.string()).optional(),
    MultiplayerModes: z.array(z.string()).optional(),
    PlayerPerspectives: z.array(z.string()).optional(),
    AfterCursor: z.string().optional(),
    PageSize: z.number().int().catch(40).optional(),
});

type SearchParams = z.infer<typeof searchQueryParamsSchema2>;

export const Route = createFileRoute("/searching")({
    component: RouteComponent,

    loader: ({ context }) => {
        const { queryClient } = context;

        // Prefetch without blocking route render; network issues should not blank the page.
        void queryClient.prefetchQuery(getApiTagsAllTagsQueryOptionsHook());
    },
    validateSearch: searchQueryParamsSchema2,
    search: {
        middlewares: [
            stripSearchParams<SearchParams>({
                Name: "",
                Genres: [],
                Themes: [],
                GameModes: [],
                MultiplayerModes: [],
                PlayerPerspectives: [],
                AfterCursor: "",
            }),
        ],
    },
});

function RouteComponent() {
    const search = Route.useSearch() ?? {};
    const navigate = Route.useNavigate();

    return (
        <SearchContainer
            contextId="search"
            initialSearch={search}
            onSearchChange={(s) => {
                navigate({
                    search: s,
                    resetScroll: false,
                });
            }}
        />
    );
}

/*
function Index() {
    const { open, onOpen, onClose } = useDisclosure();
    const [isPending, startTransition] = useTransition();
    const searchParams = Route.useSearch() ?? {};
    const navigate = Route.useNavigate();

    const onUpdate = (next: SearchQueryParams) => {
        navigate({ search: next, replace: true });
    };
    const { data, fetchNextPage, hasNextPage, isFetchingNextPage, isFetching } =
        useSearchInfiniteHook(
            { params: searchParams },
            {
                query: {
                    placeholderData: keepPreviousData,
                },
            }
        );

    return (
        <Flex
            direction="column"
            bg="bg.surface"
            minH="100vh"
            px={{ base: "md", xl: "lg" }}
            py="xl"
            gap="xl"
        >
            <Flex
                display={{ base: "flex", md: "none" }}
                justify="space-between"
            >
                <Button
                    size="sm"
                    variant="outline"
                    rounded="xl"
                    onClick={onOpen}
                >
                    Filters
                </Button>

                <Text color="fg.muted" fontSize="sm">
                    3,842 entries
                </Text>
            </Flex>

            <Flex align="flex-start" gap="xl">
                <aside>
                    <Box
                        display={{ base: "none", md: "block" }}
                        w="280px"
                        flexShrink={0}
                        position="sticky"
                        top="xl"
                    >
                        <Stack
                            bg="bg.panel"
                            rounded="2xl"
                            p="lg"
                            borderWidth="1px"
                            borderColor="border.subtle"
                            gap="lg"
                        >
                            <Flex justify="space-between" align="center">
                                <Heading size="sm">Refine</Heading>

                                <Button variant="ghost" size="xs">
                                    Reset
                                </Button>
                            </Flex>
                            {facets && (
                                <FilterSidebar
                                    facetBuckets={facets}
                                    searchParams={searchParams}
                                    onUpdate={onUpdate}
                                />
                            )}
                        </Stack>
                    </Box>
                </aside>

                <Stack flex="1" gap="xl">
                    <HStack flexWrap="wrap" gap="sm">
                        <Text fontSize="xs" color="fg.muted">
                            Filters:
                        </Text>

                        <Button size="xs" variant="subtle" rounded="full">
                            Action
                        </Button>

                        <Button size="xs" variant="subtle" rounded="full">
                            PC
                        </Button>

                        <Button size="xs" variant="subtle" rounded="full">
                            2023
                        </Button>

                        <Button size="xs" variant="ghost">
                            Clear
                        </Button>
                    </HStack>


                    <ItemScrolls
                        data={data}
                        fetchNextPage={fetchNextPage}
                        hasNextPage={hasNextPage}
                        isFetchingNextPage={isFetchingNextPage}
                        isFetching={isFetching}
                    />

                    <Flex justify="space-between" align="center">
                        <Button>Load more</Button>
                    </Flex>
                </Stack>
            </Flex>

            <Drawer.Root
                placement="block-end"
                open={open}
                onClose={onClose}
                size="xl"
                closeOnDrag
                withDragBar
            >
                <Drawer.Content>
                    <Drawer.Header>Filters</Drawer.Header>

                    <Drawer.Body>
                        <FilterFields onApply={onClose} />
                    </Drawer.Body>
                </Drawer.Content>
            </Drawer.Root>
        </Flex>
    );
}

interface ItemScrollsProps {
    data: InfiniteData<SearchQueryResponse> | undefined;
    fetchNextPage: () => Promise<
        InfiniteQueryObserverResult<InfiniteData<SearchQueryResponse>, Error>
    >;
    hasNextPage: boolean | undefined;
    isFetchingNextPage: boolean;
    isFetching: boolean;
}

export const ItemScrolls = ({
    data,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    isFetching,
}: ItemScrollsProps) => {
    const { ref, inView } = useInView();

    useEffect(() => {
        if (inView && hasNextPage && !isFetchingNextPage) {
            fetchNextPage();
        }
    }, [inView, hasNextPage, isFetchingNextPage, fetchNextPage]);

    return (
        <Grid
            gap="lg"
            templateColumns={{
                base: "repeat(2, 1fr)",
                sm: "repeat(auto-fill, minmax(160px, 1fr))",
                md: "repeat(auto-fill, minmax(200px, 1fr))",
                xl: "repeat(auto-fill, minmax(220px, 1fr))",
            }}
        >
            <For
                each={data?.pages}
                fallback={
                    <EmptyState.Root
                        description="There are no items to show"
                        indicator={<BoxIcon />}
                    />
                }
            >
                {(axiosResponse, pageIndex) => {
                    const data = axiosResponse.data;
                    const pageInfo = axiosResponse.pageInfo;

                    return (
                        <React.Fragment
                            key={
                                pageInfo?.nextPageCursor ?? `page-${pageIndex}`
                            }
                        >
                            <For each={data}>
                                {(project) => (
                                    <GridItem key={project.item?.id}>
                                        <ExplorerCard item={project.item} />
                                    </GridItem>
                                )}
                            </For>
                        </React.Fragment>
                    );
                }}
            </For>
            <GridItem display="flex" justifyContent="center" py="md">
                <button
                    ref={ref}
                    onClick={() => fetchNextPage()}
                    disabled={!hasNextPage || isFetchingNextPage}
                    style={{
                        padding: "10px",
                        cursor: hasNextPage ? "pointer" : "default",
                    }}
                >
                    {isFetchingNextPage
                        ? "Loading more..."
                        : hasNextPage
                        ? "Load More"
                        : "Nothing more to load"}
                </button>
            </GridItem>
            {isFetching && !isFetchingNextPage && (
                <GridItem textAlign="center" fontSize="xs" color="fg.muted">
                    Background Updating...
                </GridItem>
            )}
        </Grid>
    );
};


type CardProps = {
    item: GameItem | null;
};
const ExplorerCard = ({ item }: CardProps) => {
    const imageUrl = getIGDBImageUrl(item?.coverUrl, "1080p", false);

    return (
        <Card.Root
            bg="bg.panel"
            borderWidth="1px"
            borderColor="border.subtle"
            rounded="2xl"
            overflow="hidden"
            transition="all .2s"
            _hover={{
                transform: "translateY(-4px)",
                bg: "bg.muted",
            }}
        >
            <Box aspectRatio="3/4" overflow="hidden">
                <Image
                    src={imageUrl}
                    w="full"
                    h="full"
                    objectFit="cover"
                    transition="transform .4s"
                    _groupHover={{ transform: "scale(1.1)" }}
                />
            </Box>

            <Card.Body>
                <Stack gap="xs">
                    <Heading size="sm" lineClamp={2}>
                        {item?.name}
                    </Heading>

                    <Text fontSize="xs" color="fg.muted">
                        Cyberpunk • 2023
                    </Text>
                </Stack>
            </Card.Body>

            <Card.Footer>
                <Flex justify="space-between" w="full">
                    <Text fontSize="sm" fontWeight="bold">
                        ★ 9.2
                    </Text>

                    <Button size="xs" variant="ghost">
                        Save
                    </Button>
                </Flex>
            </Card.Footer>
        </Card.Root>
    );
};


interface FilterFieldsProps {
    onApply?: () => void;
}

const FilterFields = ({ onApply }: FilterFieldsProps) => (
    <Stack gap="lg" w="full" h="full">
        <Accordion.Root toggle multiple>
            <Accordion.Item button="Genre" index={0}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>Action</CheckboxGroup.Item>
                        <CheckboxGroup.Item>RPG</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Strategy</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Simulation</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Platform" index={1}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>PC</CheckboxGroup.Item>
                        <CheckboxGroup.Item>Console</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>

            <Accordion.Item button="Release Year" index={2}>
                <Accordion.Panel>
                    <CheckboxGroup.Root>
                        <CheckboxGroup.Item>2024</CheckboxGroup.Item>
                        <CheckboxGroup.Item>2023</CheckboxGroup.Item>
                        <CheckboxGroup.Item>2022</CheckboxGroup.Item>
                    </CheckboxGroup.Root>
                </Accordion.Panel>
            </Accordion.Item>
        </Accordion.Root>

        {onApply && (
            <Box pt="4" pb="calc(env(safe-area-inset-bottom) + 16px)">
                <Button w="full" size="lg" rounded="2xl" onClick={onApply}>
                    Show Results
                </Button>
            </Box>
        )}
    </Stack>
);

type FacetKey = "Themes" | "Modes" | "Genres";

const FilterSidebar = ({
    searchParams,
    facetBuckets,
    onUpdate,
}: {
    searchParams: SearchQueryParams;
    facetBuckets: FacetBuckets;
    onUpdate: (s: SearchQueryParams) => void;
}) => {
    const toggle = (key: FacetKey, val: string) => {
        const current = searchParams[key] ?? [];

        const next = current.includes(val)
            ? current.filter((v) => v !== val)
            : [...current, val];

        onUpdate({
            ...searchParams,
            [key]: next,
        });
    };

    return (
        <Accordion.Root toggle multiple>
            <Accordion.Item index={0} key="game-modes" button={"Game Modes"}>
                <Accordion.Panel>
                    {facetBuckets.gameModes
                        .filter((bucket) => bucket.key)
                        .map((bucket) => {
                            const key = bucket.key!;
                            const selected = (
                                searchParams["Modes"] ?? []
                            ).includes(key);
                            const disabled = (bucket.doc_count ?? 0) === 0;

                            return (
                                <Checkbox
                                    key={key}
                                    checked={selected}
                                    disabled={disabled}
                                    onChange={() => toggle("Modes", key)}
                                >
                                    {key} ({bucket.doc_count ?? 0})
                                </Checkbox>
                            );
                        })}
                </Accordion.Panel>
            </Accordion.Item>
        </Accordion.Root>
    );
};
*/
