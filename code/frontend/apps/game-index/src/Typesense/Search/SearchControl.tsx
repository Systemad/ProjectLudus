import { Flex, Box, Button, Select } from "ui";

export type SortFieldOption = {
    label: string;
    value: string;
};

type SortDirection = "asc" | "desc";

type SortControlsProps = {
    currentSort: string;
    sortFieldOptions: SortFieldOption[];
    onSortChange: (nextSort: string) => void;
};

export function SortControls({ currentSort, sortFieldOptions, onSortChange }: SortControlsProps) {
    const parsed = currentSort?.match(/^([^:]+):(asc|desc)$/);
    const fallbackField = sortFieldOptions[0]?.value ?? "";
    const field = parsed?.[1] ?? fallbackField;
    const direction: SortDirection = parsed?.[2] === "asc" ? "asc" : "desc";
    const isValidField = sortFieldOptions.some((option) => option.value === field);
    const displayField = isValidField ? field : fallbackField;

    const onFieldChange = (nextField: string) => {
        onSortChange(`${nextField}:${direction}`);
    };

    const onDirectionChange = (nextDirection: SortDirection) => {
        onSortChange(`${field}:${nextDirection}`);
    };

    return (
        <Flex gap="xs" wrap="nowrap" align="end" direction={{ base: "column", sm: "row" }} w="full">
            <Box minW={{ base: "100%", sm: "10rem" }}>
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
                    colorScheme="neutral"
                    onClick={() => onDirectionChange(direction === "asc" ? "desc" : "asc")}
                    minW={{ base: "100%", sm: "3rem" }}
                    w={{ base: "100%", sm: "auto" }}
                >
                    {direction === "asc" ? "↑" : "↓"}
                </Button>
            </Box>
        </Flex>
    );
}
