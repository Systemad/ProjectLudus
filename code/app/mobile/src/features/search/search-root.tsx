import { Configure, InstantSearch, useSearchBox } from "react-instantsearch-core";
import { TextInput, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";
import { useContentBottomInset } from "@/shared/ui/insets";
import { SearchResults } from "./components";
import { GAMES_SEARCH_INDEX_NAME, searchClient } from "./typesense-client";

export default function SearchRoot() {
  const colors = useAppTheme();
  const bottomInset = useContentBottomInset(20);

  return (
    <InstantSearch
      searchClient={searchClient}
      indexName={GAMES_SEARCH_INDEX_NAME}
      future={{ preserveSharedStateOnUnmount: true }}
    >
      <Configure hitsPerPage={20} />
      <SearchBody colors={colors} bottomInset={bottomInset} />
    </InstantSearch>
  );
}

function SearchBody({
  colors,
  bottomInset,
}: {
  colors: ReturnType<typeof useAppTheme>;
  bottomInset: number;
}) {
  const { query, refine } = useSearchBox();

  return (
    <View style={{ flex: 1, backgroundColor: colors.background }}>
      <TextInput
        value={query}
        onChangeText={refine}
        placeholder="Search games"
        placeholderTextColor={colors.textMuted}
        autoCapitalize="none"
        returnKeyType="search"
        style={{
          height: 52,
          marginHorizontal: 16,
          marginBottom: 8,
          borderRadius: 18,
          paddingHorizontal: 16,
          backgroundColor: colors.surfaceHigh,
          color: colors.text,
        }}
      />
      <SearchResults bottomInset={bottomInset} />
    </View>
  );
}
