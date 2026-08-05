import { Box, FloatingActionButton, Host, Icon, RNHostView } from "@expo/ui/jetpack-compose";
import { align, fillMaxSize, offset } from "@expo/ui/jetpack-compose/modifiers";

import { SearchResults } from "./search-results";

export function SearchResultsSurface({
  bottomInset,
  onFilterOpen,
}: {
  bottomInset: number;
  onFilterOpen: () => void;
}) {
  return (
    <Host style={{ flex: 1 }}>
      <Box modifiers={[fillMaxSize()]}>
        <RNHostView modifiers={[fillMaxSize()]}>
          <SearchResults bottomInset={bottomInset} />
        </RNHostView>
        <FloatingActionButton
          onClick={onFilterOpen}
          modifiers={[align("bottomEnd"), offset(-16, -16)]}
        >
          <FloatingActionButton.Icon>
            <Icon source={require("@/assets/icons/filter.xml")} contentDescription="Filter games" />
          </FloatingActionButton.Icon>
        </FloatingActionButton>
      </Box>
    </Host>
  );
}
