import { Box, Flex } from "ui";
import { useSortBy } from "react-instantsearch";
import { SortControls, type SortFieldOption } from "./SearchControl";
import { SearchInput } from "./SearchInput";

type SearchHeaderProps = {
    searchPlaceholder: string;
    indexName: string;
    sortFieldOptions: SortFieldOption[];
};

export function SearchHeader({
    searchPlaceholder,
    indexName,
    sortFieldOptions,
}: SearchHeaderProps) {
    const isItems = [
        { label: "Relevancy", value: indexName },
        ...sortFieldOptions
            .filter((o) => o.value !== "relevancy")
            .flatMap((o) => [
                { label: `${o.label} ↑`, value: `${indexName}/sort/${o.value}:asc` },
                { label: `${o.label} ↓`, value: `${indexName}/sort/${o.value}:desc` },
            ]),
    ];

    const { currentRefinement, refine } = useSortBy({ items: isItems });

    const currentSort =
        currentRefinement === indexName
            ? "relevancy"
            : currentRefinement.replace(`${indexName}/sort/`, "");

    const onSortChange = (next: string) => {
        refine(next === "relevancy" ? indexName : `${indexName}/sort/${next}`);
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
