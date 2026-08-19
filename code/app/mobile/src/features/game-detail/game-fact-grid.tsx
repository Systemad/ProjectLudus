import { Column, Text } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, padding } from "@expo/ui/jetpack-compose/modifiers";

import { MetadataGrid, type MetadataItem } from "@/shared/ui/metadata-grid";

type GameFact = { label: string; values: string[] };

export function GameFactGrid({ facts }: { facts: GameFact[] }) {
  const items: MetadataItem[] = facts
    .map((fact) => ({ label: fact.label, value: fact.values.join(" · ") }))
    .filter((fact) => fact.value.length > 0);

  if (items.length === 0) return null;

  return (
    <Column
      modifiers={[fillMaxWidth(), padding(0, 14, 0, 0)]}
      verticalArrangement={{ spacedBy: 9 }}
    >
      <Text style={{ typography: "titleLarge" }}>Game information</Text>
      <MetadataGrid columns={2} items={items} />
    </Column>
  );
}
