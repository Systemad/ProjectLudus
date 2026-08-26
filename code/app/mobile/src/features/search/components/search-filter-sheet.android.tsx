import { Host } from "@expo/ui";
import { Column, ModalBottomSheet, Text } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, paddingAll, verticalScroll } from "@expo/ui/jetpack-compose/modifiers";

import { useAppTheme } from "@/hooks/use-app-theme";
import { spacing } from "@/theme";
import { SearchFacets } from "./search-facets";

export function SearchFilterSheet({ onDismiss }: { onDismiss: () => void }) {
  const colors = useAppTheme();

  return (
    <Host matchContents seedColor={colors.primary}>
      <ModalBottomSheet
        containerColor={colors.surface}
        contentColor={colors.text}
        scrimColor={colors.background}
        onDismissRequest={onDismiss}
      >
        <Column
          verticalArrangement={{ spacedBy: spacing.xl }}
          modifiers={[fillMaxWidth(), verticalScroll(), paddingAll(spacing.xxxl)]}
        >
          <Text style={{ typography: "headlineSmall", fontWeight: "800" }}>Filter games</Text>
          <SearchFacets />
        </Column>
      </ModalBottomSheet>
    </Host>
  );
}
