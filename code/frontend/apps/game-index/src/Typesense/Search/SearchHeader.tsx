import { Box, Flex } from "ui";
import { useSortBy } from "react-instantsearch";
import { SortControls, type SortFieldOption } from "./SearchControl";
import { SearchInput } from "./SearchInput";

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
    const isItems = sortFieldOptions.flatMap((option) => [
        { label: `${option.label} ↑`, value: `${indexName}/sort/${option.value}:asc` },
        { label: `${option.label} ↓`, value: `${indexName}/sort/${option.value}:desc` },
    ]);

    const { currentRefinement, refine } = useSortBy({ items: isItems });

    const currentSort =
        currentRefinement && currentRefinement.startsWith(`${indexName}/sort/`)
            ? currentRefinement.replace(`${indexName}/sort/`, "")
            : defaultSort;

    const onSortChange = (next: string) => {
        refine(`${indexName}/sort/${next}`);
    };

    return (
        <Box mb="md">
            <Flex
                align={{ base: "stretch", md: "center" }}
                justify="space-between"
                gap="sm"
                direction={{ base: "column", md: "row" }}
            >
                <Box flex="1" minW={0}>
                    <SearchInput placeholder={searchPlaceholder} compact />
                </Box>

                <Flex
                    direction="column"
                    align={{ base: "stretch", md: "end" }}
                    gap="xs"
                    minW={{ base: "100%", md: "auto" }}
                >
                    <SortControls
                        currentSort={currentSort}
                        sortFieldOptions={sortFieldOptions}
                        onSortChange={onSortChange}
                    />
                </Flex>
            </Flex>
        </Box>
    );
}
