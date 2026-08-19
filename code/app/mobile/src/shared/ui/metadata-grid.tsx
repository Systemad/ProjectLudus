import {
  Card,
  Column,
  HorizontalDivider,
  Row,
  Spacer,
  Text,
  VerticalDivider,
} from "@expo/ui/jetpack-compose";
import {
  fillMaxHeight,
  fillMaxWidth,
  height,
  paddingAll,
  weight,
} from "@expo/ui/jetpack-compose/modifiers";
import type { ModifierConfig } from "@expo/ui/jetpack-compose/modifiers";
import { Fragment } from "react";

export type MetadataItem = {
  label: string;
  value: string;
};

export function MetadataGrid({
  items,
  columns = 2,
  modifier,
}: {
  items: MetadataItem[];
  columns?: number;
  modifier?: ModifierConfig;
}) {
  const populatedItems = items.filter((item) => item.value.length > 0);
  if (populatedItems.length === 0) return null;

  const columnCount = Number.isFinite(columns) ? Math.max(1, Math.floor(columns)) : 2;
  const rows = Array.from({ length: Math.ceil(populatedItems.length / columnCount) }, (_, index) =>
    populatedItems.slice(index * columnCount, (index + 1) * columnCount),
  );

  return (
    <Card modifiers={[fillMaxWidth(), ...(modifier ? [modifier] : [])]}>
      <Column modifiers={[fillMaxWidth()]}>
        {rows.map((row, rowIndex) => (
          <Column key={rowIndex} modifiers={[fillMaxWidth()]}>
            <Row modifiers={[fillMaxWidth()]}>
              {row.map((item, itemIndex) => (
                <Fragment key={`${item.label}-${itemIndex}`}>
                  {itemIndex > 0 ? (
                    <VerticalDivider modifiers={[height(72), fillMaxHeight()]} />
                  ) : null}
                  <MetadataCell item={item} modifier={weight(1)} />
                </Fragment>
              ))}
              {Array.from({ length: columnCount - row.length }, (_, spacerIndex) => (
                <Spacer key={`empty-${spacerIndex}`} modifiers={[weight(1)]} />
              ))}
            </Row>
            {rowIndex < rows.length - 1 ? <HorizontalDivider /> : null}
          </Column>
        ))}
      </Column>
    </Card>
  );
}

function MetadataCell({ item, modifier }: { item: MetadataItem; modifier: ModifierConfig }) {
  return (
    <Column
      modifiers={[modifier, height(72), paddingAll(12)]}
      verticalArrangement={{ spacedBy: 5 }}
    >
      <Text style={{ typography: "labelSmall" }}>{item.label}</Text>
      <Text style={{ typography: "bodyMedium" }} maxLines={2} overflow="ellipsis">
        {item.value}
      </Text>
    </Column>
  );
}
