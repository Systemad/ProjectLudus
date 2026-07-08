import { Selector } from "@astryxdesign/core/Selector";
import { Text } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { useSortBy } from "react-instantsearch";
import { Stats } from "react-instantsearch";
import type { SortFieldOption } from "./search-control";
import { SearchInput } from "./search-input";

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

    const currentSort = currentRefinement?.startsWith(`${indexName}/sort/`)
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
        <div style={{ marginBottom: "1rem" }}>
            <SearchInput placeholder={searchPlaceholder} />

            <HStack gap={3} vAlign="center" hAlign="between" wrap="wrap" style={{marginTop: "0.75rem"}}>
                <HStack gap={3} vAlign="center" wrap="wrap">
                    <Selector
                        size="sm"
                        value={field}
                        onChange={(value) => onFieldChange(value)}
                        options={sortFieldOptions.map((option) => option.value)}
                        label="Sort by"
                        isLabelHidden
                    />

                    <div
                        role="button"
                        tabIndex={0}
                        onClick={onDirectionChange}
                        onKeyDown={(e) => { if (e.key === "Enter" || e.key === " ") { e.preventDefault(); onDirectionChange(); } }}
                        style={{
                            padding: "0.375rem 0.75rem",
                            borderRadius: "var(--radius-md)",
                            fontSize: "0.875rem",
                            cursor: "pointer",
                        }}
                        aria-label={`Sort ${direction === "asc" ? "descending" : "ascending"}`}
                    >
                        {direction === "asc" ? "↑ Ascending" : "↓ Descending"}
                    </div>
                </HStack>

                <Text style={{fontSize: "0.875rem", color: "var(--fg-tertiary)"}}>
                    <Stats />
                </Text>
            </HStack>
        </div>
    );
}
