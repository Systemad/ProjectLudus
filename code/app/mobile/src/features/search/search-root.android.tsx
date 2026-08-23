import { Configure, InstantSearch, useSearchBox } from "react-instantsearch-core";
import { useState } from "react";
import { View } from "react-native";

import { Host } from "@expo/ui";
import { DockedSearchBar, RNHostView, Text } from "@expo/ui/jetpack-compose";
import { fillMaxWidth } from "@expo/ui/jetpack-compose/modifiers";
import { Search } from "lucide-react-native";

import { useAppTheme } from "@/hooks/use-app-theme";
import { useContentBottomInset } from "@/shared/ui/insets";
import { SearchFilterSheet } from "./components/search-filter-sheet.android";
import { SearchResultsSurface } from "./components/search-results-surface.android";
import { GAMES_SEARCH_INDEX_NAME, searchClient } from "./typesense-client";

export default function SearchRoot() {
  const colors = useAppTheme();
  const bottomInset = useContentBottomInset(96);
  const [filterOpen, setFilterOpen] = useState(false);

  return (
    <InstantSearch
      searchClient={searchClient}
      indexName={GAMES_SEARCH_INDEX_NAME}
      future={{ preserveSharedStateOnUnmount: true }}
    >
      <Configure hitsPerPage={20} />
      <SearchContent
        colors={colors}
        bottomInset={bottomInset}
        onFilterOpen={() => setFilterOpen(true)}
        onFilterDismiss={() => setFilterOpen(false)}
        filterOpen={filterOpen}
      />
    </InstantSearch>
  );
}

function SearchContent({
  colors,
  bottomInset,
  onFilterOpen,
  onFilterDismiss,
  filterOpen,
}: {
  colors: ReturnType<typeof useAppTheme>;
  bottomInset: number;
  filterOpen: boolean;
  onFilterOpen: () => void;
  onFilterDismiss: () => void;
}) {
  const { refine } = useSearchBox();

  return (
    <View style={{ flex: 1, backgroundColor: colors.background }}>
      <View style={{ paddingHorizontal: 16, paddingBottom: 8 }}>
        <Host matchContents style={{ width: "100%" }}>
          <DockedSearchBar onQueryChange={refine} modifiers={[fillMaxWidth()]}>
            <DockedSearchBar.Placeholder>
              <Text>Search games</Text>
            </DockedSearchBar.Placeholder>
            <DockedSearchBar.LeadingIcon>
              <RNHostView matchContents>
                <Search color={colors.text} size={20} strokeWidth={2.2} />
              </RNHostView>
            </DockedSearchBar.LeadingIcon>
          </DockedSearchBar>
        </Host>
      </View>
      <SearchResultsSurface bottomInset={bottomInset} onFilterOpen={onFilterOpen} />
      {filterOpen ? <SearchFilterSheet onDismiss={onFilterDismiss} /> : null}
    </View>
  );
}
