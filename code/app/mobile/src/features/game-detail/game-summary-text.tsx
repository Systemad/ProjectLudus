import { useState } from "react";
import { StyleSheet } from "react-native";
import Animated, { LinearTransition } from "react-native-reanimated";

import { useAppTheme } from "@/hooks/use-app-theme";

const SUMMARY_LINES = 4;
const SUMMARY_TRANSITION = LinearTransition.duration(180);

export function GameSummaryText({ summary }: { summary: string }) {
  const colors = useAppTheme();
  const [expanded, setExpanded] = useState(false);

  return (
    <Animated.Text
      accessibilityHint={expanded ? "Collapses the summary" : "Expands the summary"}
      accessibilityLabel="Game summary"
      accessibilityRole="button"
      accessibilityState={{ expanded }}
      layout={SUMMARY_TRANSITION}
      numberOfLines={expanded ? undefined : SUMMARY_LINES}
      onPress={() => setExpanded((current) => !current)}
      selectable
      style={[styles.text, { color: colors.textMuted }]}
    >
      {summary}
    </Animated.Text>
  );
}

const styles = StyleSheet.create({
  text: {
    fontSize: 15,
    lineHeight: 22,
  },
});
