import { StyleSheet, Text, View } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { useAppTheme } from "@/hooks/use-app-theme";
import { commonStyles } from "@/shared/ui/common-styles";

type GameFact = {
  label: string;
  values: string[];
  wide?: boolean;
};

export function GameFactGrid({ facts }: { facts: GameFact[] }) {
  const colors = useAppTheme();
  const populatedFacts = facts.filter((fact) => fact.values.length > 0);

  if (populatedFacts.length === 0) {
    return null;
  }

  return (
    <View style={styles.grid}>
      {populatedFacts.map((fact) => (
        <View
          key={fact.label}
          style={[
            commonStyles.surfaceCard,
            styles.fact,
            {
              backgroundColor: colors.surfaceHigh,
              flexBasis: fact.wide ? "100%" : "47%",
            },
          ]}
        >
          <Text style={[styles.label, { color: colors.textMuted }]}>
            {fact.label.toUpperCase()}
          </Text>
          <Text selectable style={[styles.value, { color: colors.text }]}>
            {fact.values.join(" · ")}
          </Text>
        </View>
      ))}
    </View>
  );
}

const styles = StyleSheet.create({
  grid: {
    paddingHorizontal: PAGE_GUTTER,
    flexDirection: "row",
    flexWrap: "wrap",
    gap: 10,
  },
  fact: {
    flexGrow: 1,
    minWidth: 0,
    padding: 14,
    gap: 5,
  },
  label: {
    fontSize: 11,
    lineHeight: 14,
    fontWeight: "800",
    letterSpacing: 0.8,
  },
  value: {
    fontSize: 14,
    lineHeight: 20,
    fontWeight: "600",
  },
});
