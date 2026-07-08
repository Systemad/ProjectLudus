import { useDebouncedCallback } from "@mantine/hooks";
import { HStack } from "@astryxdesign/core/HStack";
import { TextInput } from "@astryxdesign/core/TextInput";
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
        <HStack
            className={compact ? undefined : "typesense-searchbox"}
            gap={3}
            vAlign="center"
            wrap={compact ? "nowrap" : "wrap"}
            style={compact ? {} : {marginBottom: "0"}}
        >
            <TextInput
                label={placeholder}
                isLabelHidden
                placeholder={placeholder}
                value={deferredInputValue}
                onChange={(nextValue) => {
                    setInputValue(nextValue);
                    debouncedRefine(nextValue);
                }}
                size="lg"
                hasClear
            />
        </HStack>
    );
}
