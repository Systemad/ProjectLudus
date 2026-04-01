import { useDebouncedCallback } from "@mantine/hooks";
import { Flex, Input, Button } from "ui";
import { useState } from "react";
import { useSearchBox } from "react-instantsearch";

const SEARCH_DEBOUNCE_MS = 180;

export function SearchBox() {
    const { query, refine, clear } = useSearchBox();
    const [inputValue, setInputValue] = useState(query);
    const debouncedRefine = useDebouncedCallback(
        (value: string) => refine(value),
        SEARCH_DEBOUNCE_MS,
    );

    return (
        <Flex className="typesense-searchbox" gap="sm" align="center">
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
            />

            <Button
                variant="outline"
                size="lg"
                rounded="xl"
                onClick={() => {
                    setInputValue("");
                    clear();
                }}
                disabled={!inputValue}
            >
                Clear
            </Button>
        </Flex>
    );
}
