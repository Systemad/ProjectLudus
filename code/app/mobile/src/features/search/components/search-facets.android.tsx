import { Checkbox, Column, Row, Text } from "@expo/ui/jetpack-compose";
import { clickable, fillMaxWidth, padding } from "@expo/ui/jetpack-compose/modifiers";
import { useRefinementList } from "react-instantsearch-core";

import { gameFacets } from "../search-config";

export function SearchFacets() {
  return (
    <Column verticalArrangement={{ spacedBy: 20 }} modifiers={[fillMaxWidth()]}>
      {gameFacets.map((facet) => (
        <FacetSection key={facet.attribute} attribute={facet.attribute} label={facet.label} />
      ))}
    </Column>
  );
}

function FacetSection({ attribute, label }: { attribute: string; label: string }) {
  const { items, refine } = useRefinementList({
    attribute,
    limit: 100,
  });

  return (
    <Column verticalArrangement={{ spacedBy: 8 }} modifiers={[fillMaxWidth()]}>
      <Text style={{ typography: "titleMedium", fontWeight: "800" }}>{label}</Text>
      {items.map((item) => (
        <Row
          key={item.value}
          verticalAlignment="center"
          modifiers={[fillMaxWidth(), padding(0, 4, 0, 4), clickable(() => refine(item.value))]}
        >
          <Checkbox
            value={item.isRefined}
            onCheckedChange={(value) => {
              if (value !== item.isRefined) refine(item.value);
            }}
          />
          <Text style={{ typography: "bodyLarge" }}>{`${item.label} (${item.count})`}</Text>
        </Row>
      ))}
      {items.length === 0 ? <Text>No filter options available</Text> : null}
    </Column>
  );
}
