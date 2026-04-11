import { Flex, Box, Text, Button } from "ui";
import { useSearch, useNavigate } from "@tanstack/react-router";
import { Route } from "@src/routes/search";

const SORT_FIELD_OPTIONS = [
    { label: "Relevancy", value: "relevancy" },
    { label: "First Release Date", value: "first_release_date" },
] as const;

const SORT_DIRECTION_OPTIONS = [
    { label: "Ascending", value: "asc" },
    { label: "Descending", value: "desc" },
] as const;

type SortField = (typeof SORT_FIELD_OPTIONS)[number]["value"];
type SortDirection = (typeof SORT_DIRECTION_OPTIONS)[number]["value"];

const RELEVANCY_SORT = "aggregated_rating:desc";

export function SortControls() {
    const search = useSearch({ from: Route.fullPath });
    const navigate = useNavigate({ from: Route.fullPath });

    const currentSort = search.sort ?? RELEVANCY_SORT;
    const parsed = currentSort.match(/^([^:]+):(asc|desc)$/);

    const field: SortField =
        parsed?.[1] === "first_release_date" ? "first_release_date" : "relevancy";
    const direction: SortDirection = parsed?.[2] === "asc" ? "asc" : "desc";
    const isSortableField = field !== "relevancy";

    const setSort = (nextSort: string) => {
        navigate({
            search: (prev) => ({ ...prev, sort: nextSort, page: 1 }),
            replace: true,
        });
    };

    const onFieldChange = (nextField: SortField) => {
        if (nextField === "relevancy") {
            setSort(RELEVANCY_SORT);
            return;
        }

        setSort(`${nextField}:${direction}`);
    };

    const onDirectionChange = (nextDirection: SortDirection) => {
        if (!isSortableField) return;
        setSort(`${field}:${nextDirection}`);
    };

    return (
        <Flex gap="sm" wrap="wrap" mb="sm" align="end">
            <Box minW={{ base: "100%", sm: "13rem" }}>
                <Text fontSize="xs" color="fg.muted" mb="xs">
                    Sort field
                </Text>
                <Flex gap="xs" wrap="wrap">
                    {SORT_FIELD_OPTIONS.map((option) => (
                        <Button
                            key={option.value}
                            size="sm"
                            variant={field === option.value ? "solid" : "outline"}
                            onClick={() => onFieldChange(option.value)}
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
                                direction === option.value && isSortableField ? "solid" : "outline"
                            }
                            disabled={!isSortableField}
                            onClick={() => onDirectionChange(option.value)}
                        >
                            {option.label}
                        </Button>
                    ))}
                </Flex>
            </Box>
        </Flex>
    );
}
