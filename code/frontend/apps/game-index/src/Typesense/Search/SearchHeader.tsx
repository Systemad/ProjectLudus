import { Box } from "ui";
import { SortControls } from "./SearchControl";
import { SearchInput } from "./SearchInput";

export function SearchHeader() {
    return (
        <Box mb="md">
            <SearchInput />

            <SortControls />
        </Box>
    );
}
