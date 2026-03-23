import { Box } from "@packages/ui";
import { SearchBox } from "react-instantsearch";
import { SortControls } from "./SearchControl";

export function SearchHeader() {
    return (
        <Box>
            <SearchBox />

            <SortControls />
        </Box>
    );
}
