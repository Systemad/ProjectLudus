import { Checkbox, Column, Text } from "@expo/ui";
import { useRefinementList } from "react-instantsearch-core";

import { gameFacets } from "../search-config";

export function SearchFacets() {
  return (
    <Column spacing={18}>
      {gameFacets.map((facet) => (
        <FacetSection key={facet.attribute} attribute={facet.attribute} label={facet.label} />
      ))}
    </Column>
  );
}

function FacetSection({ attribute, label }: { attribute: string; label: string }) {
  const { items, refine } = useRefinementList({
    attribute,
    limit: 8,
    showMore: true,
    showMoreLimit: 30,
  });

  return (
    <Column spacing={8}>
      <Text style={{ fontSize: 15, fontWeight: "800" }}>{label}</Text>
      {items.map((item) => (
        <Checkbox
          key={item.value}
          label={`${item.label} (${item.count})`}
          value={item.isRefined}
          onValueChange={() => refine(item.value)}
        />
      ))}
      {items.length === 0 ? <Text>No filter options available</Text> : null}
    </Column>
  );
}
