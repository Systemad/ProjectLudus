import { useDebouncedCallback } from "@mantine/hooks";
import { Flex, Input, Button } from "ui";
import { useState } from "react";
import { useSearchBox } from "react-instantsearch";

const SEARCH_DEBOUNCE_MS = 180;

export function SearchInput() {
    const { query, refine, clear } = useSearchBox();
    const [inputValue, setInputValue] = useState(query);
    const debouncedRefine = useDebouncedCallback(
        (value: string) => refine(value),
        SEARCH_DEBOUNCE_MS,
    );

    return (
        <Flex className="typesense-searchbox" gap="sm" align="center" wrap="wrap">
            <Input
                placeholder="Search games..."
                value={inputValue}
                onChange={(event) => {
                    const nextValue = event.currentTarget.value;
                    setInputValue(nextValue);
                    debouncedRefine(nextValue);
                }}
                size="lg"
                rounded="xl"
                flex="1"
                minW={{ base: "100%", sm: "18rem" }}
                bg="transparentize(gray.900, 34%)"
                borderColor="whiteAlpha.200"
                backdropFilter="blur(16px) saturate(115%)"
                boxShadow="inset 0 1px 0 {colors.whiteAlpha.150}, inset 0 0 0 1px {colors.whiteAlpha.100}"
            />

            <Button
                variant="ghost"
                size="lg"
                rounded="xl"
                px="lg"
                bg="transparentize(gray.900, 38%)"
                borderWidth="1px"
                borderColor="whiteAlpha.150"
                onClick={() => {
                    setInputValue("");
                    debouncedRefine.cancel?.();
                    clear();
                }}
                disabled={!inputValue}
            >
                Clear
            </Button>
        </Flex>
    );
}
