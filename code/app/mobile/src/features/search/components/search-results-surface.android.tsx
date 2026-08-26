import { FloatingActionButton, Host, Icon } from "@expo/ui/jetpack-compose";
import { StyleSheet, View } from "react-native";

import { SearchResults } from "./search-results";

export function SearchResultsSurface({
  bottomInset,
  onFilterOpen,
}: {
  bottomInset: number;
  onFilterOpen: () => void;
}) {
  return (
    <View style={styles.container}>
      <SearchResults bottomInset={bottomInset} />
      <Host matchContents style={styles.filterHost}>
        <FloatingActionButton onClick={onFilterOpen}>
          <FloatingActionButton.Icon>
            <Icon source={require("@/assets/icons/filter.xml")} contentDescription="Filter games" />
          </FloatingActionButton.Icon>
        </FloatingActionButton>
      </Host>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  filterHost: {
    bottom: 16,
    position: "absolute",
    right: 16,
  },
});
