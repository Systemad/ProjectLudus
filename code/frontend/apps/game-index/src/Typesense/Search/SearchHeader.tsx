import { Box, Flex, Select, Text } from "ui";
import { useSortBy } from "react-instantsearch";
import type { SortFieldOption } from "./SearchControl";
import { SearchInput } from "./SearchInput";
import { Stats } from "react-instantsearch";

type SearchHeaderProps = {
    searchPlaceholder: string;
    indexName: string;
    sortFieldOptions: SortFieldOption[];
    defaultSort: string;
};

export function SearchHeader({
    searchPlaceholder,
    indexName,
    sortFieldOptions,
    defaultSort,
}: SearchHeaderProps) {
    const items = sortFieldOptions.flatMap((option) => [
        { label: `${option.label} ↑`, value: `${indexName}/sort/${option.value}:asc` },
        { label: `${option.label} ↓`, value: `${indexName}/sort/${option.value}:desc` },
    ]);

    const { currentRefinement, refine } = useSortBy({ items });

    const currentSort =
        currentRefinement?.startsWith(`${indexName}/sort/`)
            ? currentRefinement.replace(`${indexName}/sort/`, "")
            : defaultSort;

    const onSortChange = (next: string) => {
        refine(`${indexName}/sort/${next}`);
    };

    const parsed = currentSort?.match(/^([^:]+):(asc|desc)$/);
    const field = parsed?.[1] ?? defaultSort;
    const direction: "asc" | "desc" = parsed?.[2] === "asc" ? "asc" : "desc";

    const onFieldChange = (nextField: string) => {
        onSortChange(`${nextField}:${direction}`);
    };

    const onDirectionChange = () => {
        const next = direction === "asc" ? "desc" : "asc";
        onSortChange(`${field}:${next}`);
    };

    return (
        <Box mb="md">
            <SearchInput placeholder={searchPlaceholder} />

            <Flex
                gap="sm"
                align="center"
                justify="space-between"
                wrap="wrap"
                mt="3"
            >
                <Flex gap="sm" align="center" wrap="wrap">
                    <Select.Root
                        size="sm"
                        value={field}
                        onChange={(value) => onFieldChange(value)}
                        items={sortFieldOptions.map((option) => ({
                            label: option.label,
                            value: option.value,
                        }))}
                        aria-label="Sort by"
                    />

                    <Box
                        as="button"
                        type="button"
                        onClick={onDirectionChange}
                        px="3"
                        py="1.5"
                        rounded="md"
                        bg="bg.subtle"
                        fontSize="sm"
                        color="fg.muted"
                        _hover={{ bg: "bg.muted" }}
                        aria-label={`Sort ${direction === "asc" ? "descending" : "ascending"}`}
                    >
                        {direction === "asc" ? "↑ Ascending" : "↓ Descending"}
                    </Box>
                </Flex>

                <Text fontSize="sm" color="fg.subtle">
                    <Stats />
                </Text>
            </Flex>
        </Box>
    );
}
