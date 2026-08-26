import { Host, Row, ScrollView } from "@expo/ui";
import { useWindowDimensions } from "react-native";
import type { ReactNode } from "react";

import { GAME_RAIL_GAP } from "@/config/layout";

export function LastVisitedCarousel({ children }: { children: ReactNode }) {
  const { width } = useWindowDimensions();

  return (
    <Host matchContents={{ vertical: true }} style={{ width }}>
      <ScrollView direction="horizontal" showsIndicators={false}>
        <Row spacing={GAME_RAIL_GAP}>{children}</Row>
      </ScrollView>
    </Host>
  );
}
