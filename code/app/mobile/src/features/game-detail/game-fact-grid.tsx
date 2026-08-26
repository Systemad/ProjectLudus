import { StyleSheet, Text, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";
import { MetadataGrid, type MetadataItem } from "@/shared/ui/metadata-grid";
import { spacing, typography } from "@/theme";

type GameFact = { label: string; values: string[] };

export function GameFactGrid({ facts }: { facts: GameFact[] }) {
  const colors = useAppTheme();
  const items: MetadataItem[] = facts
    .map((fact) => ({ label: fact.label, value: fact.values.join(" · ") }))
    .filter((fact) => fact.value.length > 0);

  if (items.length === 0) return null;

  return (
    <View style={styles.section}>
      <Text style={[styles.title, { color: colors.text }]}>Game information</Text>
      <MetadataGrid columns={2} items={items} />
    </View>
  );
}

const styles = StyleSheet.create({
  section: {
    gap: spacing.sm,
  },
  title: {
    ...typography.sectionTitle,
    fontSize: 19,
  },
});
