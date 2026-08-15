import { Host } from "@expo/ui";
import { Card, Column, FlowRow, Text } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, paddingAll, width } from "@expo/ui/jetpack-compose/modifiers";
import { useWindowDimensions } from "react-native";

type GameFact = {
  label: string;
  values: string[];
  wide?: boolean;
};

export function GameFactGrid({ facts }: { facts: GameFact[] }) {
  const { width: viewportWidth } = useWindowDimensions();
  const cardWidth = (viewportWidth - 32 - 10) / 2;
  const populatedFacts = facts.filter((fact) => fact.values.length > 0);

  if (populatedFacts.length === 0) return null;

  return (
    <Host matchContents={{ vertical: true }} style={{ width: "100%" }}>
      <FlowRow
        modifiers={[fillMaxWidth()]}
        horizontalArrangement={{ spacedBy: 10 }}
        verticalArrangement={{ spacedBy: 10 }}
      >
        {populatedFacts.map((fact) => (
          <Card key={fact.label} modifiers={[width(fact.wide ? viewportWidth - 32 : cardWidth)]}>
            <Column modifiers={[paddingAll(14)]} verticalArrangement={{ spacedBy: 5 }}>
              <Text style={{ typography: "labelSmall", fontWeight: "700" }}>{fact.label}</Text>
              <Text style={{ typography: "bodyMedium", fontWeight: "600" }}>
                {fact.values.join(" · ")}
              </Text>
            </Column>
          </Card>
        ))}
      </FlowRow>
    </Host>
  );
}
