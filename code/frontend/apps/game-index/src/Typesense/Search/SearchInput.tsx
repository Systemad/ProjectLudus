import { useDebouncedCallback } from "@mantine/hooks";
import { Flex, Input } from "ui";
import { useEffect, useState } from "react";
import { useSearchBox } from "react-instantsearch";

const SEARCH_DEBOUNCE_MS = 250;

type SearchInputProps = {
    placeholder?: string;
    compact?: boolean;
};

export function SearchInput({ placeholder = "Search...", compact = false }: SearchInputProps) {
    const { query, refine } = useSearchBox();
    const [inputValue, setInputValue] = useState(query);
    const debouncedRefine = useDebouncedCallback((v: string) => refine(v), SEARCH_DEBOUNCE_MS);

    useEffect(() => {
        setInputValue(query);
    }, [query]);

    return (
        <Flex
            className={compact ? undefined : "typesense-searchbox"}
            gap="sm"
            align="center"
            wrap={compact ? "nowrap" : "wrap"}
            mb={compact ? "0" : undefined}
        >
            <Input
                placeholder={placeholder}
                value={inputValue}
                onChange={(event) => {
                    const nextValue = event.currentTarget.value;
                    setInputValue(nextValue);
                    debouncedRefine(nextValue);
                }}
                size="lg"
                rounded="xl"
                flex="1"
                minW={compact ? "0" : { base: "100%", sm: "18rem" }}
                bg="transparentize(gray.900, 34%)"
                borderColor="whiteAlpha.200"
                backdropFilter="blur(16px) saturate(115%)"
                boxShadow="inset 0 1px 0 {colors.whiteAlpha.150}, inset 0 0 0 1px {colors.whiteAlpha.100}"
            />
        </Flex>
    );
}
