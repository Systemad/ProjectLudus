import { ScrollView, StyleSheet } from "react-native";
import type { ReactNode } from "react";

import { GAME_RAIL_GAP } from "@/config/layout";

export function LastVisitedCarousel({ children }: { children: ReactNode }) {
  return (
    <ScrollView
      horizontal
      showsHorizontalScrollIndicator={false}
      contentContainerStyle={styles.content}
    >
      {children}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  content: {
    gap: GAME_RAIL_GAP,
  },
});
