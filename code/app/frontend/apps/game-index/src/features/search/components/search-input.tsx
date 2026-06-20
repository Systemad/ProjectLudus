import { useDebouncedCallback } from "@mantine/hooks";
import { Flex, Input } from "ui";
import { useDeferredValue, useState } from "react";
import { useSearchBox } from "react-instantsearch";

const SEARCH_DEBOUNCE_MS = 250;

type SearchInputProps = {
    placeholder?: string;
    compact?: boolean;
};

export function SearchInput({ placeholder = "Search...", compact = false }: SearchInputProps) {
    const { query, refine } = useSearchBox();
    const [inputValue, setInputValue] = useState(query);
    const deferredInputValue = useDeferredValue(inputValue);
    const debouncedRefine = useDebouncedCallback((v: string) => refine(v), SEARCH_DEBOUNCE_MS);

    return (
        <Flex
            className={compact ? undefined : "typesense-searchbox"}
            gap="sm"
            align="center"
            wrap={compact ? "nowrap" : "wrap"}
            mb={compact ? "0" : undefined}
        >
            <Input
                aria-label={placeholder}
                placeholder={placeholder}
                value={deferredInputValue}
                colorScheme="gray"
                onChange={(event) => {
                    const nextValue = event.currentTarget.value;
                    setInputValue(nextValue);
                    debouncedRefine(nextValue);
                }}
                size="xl"
                variant="filled"
                rounded="xl"
                flex="1"
                minW={compact ? "0" : { base: "100%", sm: "18rem" }}
            />
        </Flex>
    );
}
