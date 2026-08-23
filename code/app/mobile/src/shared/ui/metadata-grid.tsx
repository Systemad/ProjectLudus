import { StyleSheet, Text, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";

export type MetadataItem = {
  label: string;
  value: string;
};

export function MetadataGrid({ items, columns = 2 }: { items: MetadataItem[]; columns?: number }) {
  const colors = useAppTheme();
  const populatedItems = items.filter((item) => item.value.length > 0);
  if (populatedItems.length === 0) return null;

  const columnCount = Number.isFinite(columns) ? Math.max(1, Math.floor(columns)) : 2;
  const rows = Array.from({ length: Math.ceil(populatedItems.length / columnCount) }, (_, index) =>
    populatedItems.slice(index * columnCount, (index + 1) * columnCount),
  );

  return (
    <View
      style={[styles.card, { backgroundColor: colors.surfaceHigh, borderColor: colors.outline }]}
    >
      {rows.map((row, rowIndex) => (
        <View
          key={`row-${rowIndex}`}
          style={[
            styles.row,
            rowIndex > 0 && {
              borderTopColor: colors.outline,
              borderTopWidth: StyleSheet.hairlineWidth,
            },
          ]}
        >
          {row.map((item, itemIndex) => (
            <View
              key={`${item.label}-${itemIndex}`}
              style={[
                styles.cell,
                itemIndex > 0 && {
                  borderLeftColor: colors.outline,
                  borderLeftWidth: StyleSheet.hairlineWidth,
                },
              ]}
            >
              <Text style={[styles.label, { color: colors.textMuted }]}>{item.label}</Text>
              <Text selectable numberOfLines={2} style={[styles.value, { color: colors.text }]}>
                {item.value}
              </Text>
            </View>
          ))}
          {row.length < columnCount
            ? Array.from({ length: columnCount - row.length }, (_, index) => (
                <View key={`empty-${index}`} style={styles.cell} />
              ))
            : null}
        </View>
      ))}
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    borderRadius: 16,
    borderWidth: StyleSheet.hairlineWidth,
    borderCurve: "continuous",
    overflow: "hidden",
  },
  row: {
    flexDirection: "row",
  },
  cell: {
    alignItems: "center",
    flex: 1,
    gap: 5,
    justifyContent: "center",
    minHeight: 76,
    padding: 12,
  },
  label: {
    fontSize: 11,
    fontWeight: "700",
    letterSpacing: 0.2,
    textAlign: "center",
  },
  value: {
    fontSize: 14,
    fontWeight: "600",
    lineHeight: 19,
    textAlign: "center",
  },
});
