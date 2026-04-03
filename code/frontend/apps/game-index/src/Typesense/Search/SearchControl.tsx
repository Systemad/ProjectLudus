import { Flex, Box, Button, Text } from "ui";
import { useSortBy } from "react-instantsearch";
import { SEARCH_INDEX_NAME } from "../instantsearch";

type SortAnchor = (typeof SORT_ANCHOR_OPTIONS)[number]["value"];
type SortDirection = (typeof SORT_DIRECTION_OPTIONS)[number]["value"];

const SORT_ANCHOR_OPTIONS = [
    { label: "Relevancy", value: "relevancy" },
    { label: "Release date", value: "first_release_date" },
] as const;

const SORT_DIRECTION_OPTIONS = [
    { label: "Ascending", value: "asc" },
    { label: "Descending", value: "desc" },
] as const;

const RELEVANCY_SORT_ITEM = {
    label: "Relevancy",
    value: SEARCH_INDEX_NAME,
};

const ALL_SORT_ITEMS = [
    RELEVANCY_SORT_ITEM,
    ...SORT_ANCHOR_OPTIONS.filter((option) => option.value !== "relevancy").flatMap((anchor) =>
        SORT_DIRECTION_OPTIONS.map((direction) => ({
            label: `${anchor.label} ${direction.label}`,
            value: `${SEARCH_INDEX_NAME}/sort/${anchor.value}:${direction.value}`,
        })),
    ),
];

export function SortControls() {
    const { currentRefinement, refine } = useSortBy({ items: ALL_SORT_ITEMS });

    const match = currentRefinement.match(/\/sort\/([^:]+):(asc|desc)$/);
    const anchor =
        currentRefinement === SEARCH_INDEX_NAME
            ? "relevancy"
            : ((match?.[1] ?? "relevancy") as SortAnchor);
    const direction = (match?.[2] ?? "desc") as SortDirection;
    const isReplicaSort = anchor !== "relevancy";

    return (
        <Flex gap="sm" wrap="wrap" mb="sm" align="end">
            <Box minW={{ base: "100%", sm: "13rem" }}>
                <Text fontSize="xs" color="fg.muted" mb="xs">
                    Sort field
                </Text>
                <Flex gap="xs" wrap="wrap">
                    {SORT_ANCHOR_OPTIONS.map((option) => (
                        <Button
                            key={option.value}
                            size="sm"
                            variant={anchor === option.value ? "solid" : "outline"}
                            onClick={() => {
                                if (option.value === "relevancy") {
                                    refine(SEARCH_INDEX_NAME);
                                    return;
                                }

                                refine(`${SEARCH_INDEX_NAME}/sort/${option.value}:${direction}`);
                            }}
                        >
                            {option.label}
                        </Button>
                    ))}
                </Flex>
            </Box>

            <Box minW={{ base: "100%", sm: "11rem" }}>
                <Text fontSize="xs" color="fg.muted" mb="xs">
                    Direction
                </Text>
                <Flex gap="xs" wrap="wrap">
                    {SORT_DIRECTION_OPTIONS.map((option) => (
                        <Button
                            key={option.value}
                            size="sm"
                            variant={
                                direction === option.value && isReplicaSort ? "solid" : "outline"
                            }
                            disabled={!isReplicaSort}
                            onClick={() => {
                                if (!isReplicaSort) return;

                                refine(`${SEARCH_INDEX_NAME}/sort/${anchor}:${option.value}`);
                            }}
                        >
                            {option.label}
                        </Button>
                    ))}
                </Flex>
            </Box>
        </Flex>
    );
}
