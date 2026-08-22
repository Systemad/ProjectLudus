import { useState } from "react";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";

const SUMMARY_LINES = 3;

export function GameSummaryCard({ summary }: { summary: string }) {
  const colors = useAppTheme();
  const [expanded, setExpanded] = useState(false);

  return (
    <View style={[styles.card, { backgroundColor: colors.surfaceHigh }]}>
      <Text style={[styles.title, { color: colors.text }]}>Summary</Text>
      <Text
        selectable
        numberOfLines={expanded ? undefined : SUMMARY_LINES}
        style={[styles.summary, { color: colors.textMuted }]}
      >
        {summary}
      </Text>
      <Pressable
        accessibilityRole="button"
        accessibilityLabel={expanded ? "Show less summary" : "Read more summary"}
        onPress={() => setExpanded((value) => !value)}
        style={({ pressed }) => [styles.toggle, { opacity: pressed ? 0.68 : 1 }]}
      >
        <Text style={[styles.toggleText, { color: colors.primary }]}>
          {expanded ? "Show less" : "Read more"}
        </Text>
      </Pressable>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    borderRadius: 16,
    borderCurve: "continuous",
    gap: 10,
    padding: 16,
  },
  title: {
    fontSize: 19,
    fontWeight: "800",
  },
  summary: {
    fontSize: 15,
    lineHeight: 22,
  },
  toggle: {
    alignItems: "center",
    minHeight: 36,
    justifyContent: "center",
  },
  toggleText: {
    fontSize: 14,
    fontWeight: "800",
  },
});
