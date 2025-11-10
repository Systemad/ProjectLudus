import { useDebouncedCallback } from "@mantine/hooks";
import { Input } from "@yamada-ui/react";
import { useState } from "react";

export function GameSearchBox({
    query,
    onSearch,
}: {
    query: string;
    onSearch: (val: string) => void;
}) {
    const [value, setValue] = useState<string>("");

    const handleSearch = useDebouncedCallback(
        (next: string) => onSearch(next),
        { delay: 500 }
    );

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const next = e.currentTarget.value;
        setValue(next);
        handleSearch(next);
    };

    return (
        <Input
            placeholder="Search games"
            defaultValue={query}
            value={value}
            onChange={handleChange}
        />
    );
}
