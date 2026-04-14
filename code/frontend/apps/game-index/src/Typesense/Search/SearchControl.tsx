import { Flex, Box, Button, Select } from "ui";

export type SortFieldOption = {
    label: string;
    value: string;
};

const DEFAULT_SORT_FIELD_OPTIONS: SortFieldOption[] = [{ label: "Relevancy", value: "relevancy" }];
type SortDirection = "asc" | "desc";

type SortControlsProps = {
    currentSort?: string;
    sortFieldOptions?: SortFieldOption[];
    onSortChange: (nextSort: string) => void;
};

export function SortControls({
    currentSort,
    sortFieldOptions = DEFAULT_SORT_FIELD_OPTIONS,
    onSortChange,
}: SortControlsProps) {
    const parsed = currentSort?.match(/^([^:]+):(asc|desc)$/);
    const field: string = parsed?.[1] ?? "relevancy";
    const direction: SortDirection = parsed?.[2] === "asc" ? "asc" : "desc";
    const isSortableField = field !== "relevancy";

    const isValidField =
        field === "relevancy" || sortFieldOptions.some((option) => option.value === field);
    const displayField = isValidField ? field : "relevancy";

    const onFieldChange = (nextField: string) => {
        if (nextField === "relevancy") {
            onSortChange("relevancy");
            return;
        }

        onSortChange(`${nextField}:${direction}`);
    };

    const onDirectionChange = (nextDirection: SortDirection) => {
        if (!isSortableField) return;
        onSortChange(`${field}:${nextDirection}`);
    };

    return (
        <Flex gap="sm" wrap="nowrap" align="end">
            <Box minW={{ base: "10rem", sm: "13rem" }}>
                <Select.Root
                    size="sm"
                    value={displayField}
                    onChange={(value) => onFieldChange(value)}
                    items={sortFieldOptions.map((option) => ({
                        label: option.label,
                        value: option.value,
                    }))}
                />
            </Box>

            <Box>
                <Button
                    size="sm"
                    variant="outline"
                    disabled={!isSortableField}
                    onClick={() => onDirectionChange(direction === "asc" ? "desc" : "asc")}
                    minW="3rem"
                >
                    {direction === "asc" ? "↑" : "↓"}
                </Button>
            </Box>
        </Flex>
    );
}
