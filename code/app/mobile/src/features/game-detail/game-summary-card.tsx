import { Card, Column, Text } from "@expo/ui/jetpack-compose";
import { clickable, fillMaxWidth, padding, paddingAll } from "@expo/ui/jetpack-compose/modifiers";
import { useState } from "react";

const SUMMARY_LINES = 3;

export function GameSummaryCard({ summary }: { summary: string }) {
  const [expanded, setExpanded] = useState(false);

  return (
    <Column modifiers={[fillMaxWidth(), padding(0, 9, 0, 9)]}>
      <Card modifiers={[fillMaxWidth()]}>
        <Column modifiers={[fillMaxWidth(), paddingAll(16)]} verticalArrangement={{ spacedBy: 8 }}>
          <Text style={{ typography: "titleLarge" }}>Summary</Text>
          <Text
            maxLines={expanded ? undefined : SUMMARY_LINES}
            overflow="ellipsis"
            style={{ typography: "bodyMedium" }}
          >
            {summary}
          </Text>
          <Text
            modifiers={[fillMaxWidth(), clickable(() => setExpanded((value) => !value))]}
            style={{ typography: "labelLarge", textAlign: "center" }}
          >
            {expanded ? "Show less" : "Read more"}
          </Text>
        </Column>
      </Card>
    </Column>
  );
}
