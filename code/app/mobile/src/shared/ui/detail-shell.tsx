import { Image } from "expo-image";
import type { ReactNode } from "react";
import { ScrollView, StyleSheet, Text, View } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";
import { useAppTheme } from "@/hooks/use-app-theme";
import { radius, spacing, typography } from "@/theme";
import { commonStyles } from "./common-styles";

type DetailShellProps = {
  title: string;
  eyebrow: string;
  summary: string;
  imageUrl?: string;
  children: ReactNode;
};

type DetailFact = {
  label: string;
  values: string[];
};

export function DetailShell({ title, eyebrow, summary, imageUrl, children }: DetailShellProps) {
  const colors = useAppTheme();

  return (
    <ScrollView
      style={{ backgroundColor: colors.background }}
      contentContainerStyle={styles.content}
      contentInsetAdjustmentBehavior="automatic"
    >
      <View style={styles.header}>
        {imageUrl ? (
          <Image source={imageUrl} style={styles.cover} contentFit="cover" transition={200} />
        ) : null}
        <View style={styles.heading}>
          <Text style={[styles.eyebrow, { color: colors.primary }]}>{eyebrow.toUpperCase()}</Text>
          <Text selectable style={[styles.title, { color: colors.text }]}>
            {title}
          </Text>
        </View>
      </View>
      <Text selectable style={[styles.summary, { color: colors.textMuted }]}>
        {summary}
      </Text>
      {children}
    </ScrollView>
  );
}

export function DetailFacts({ facts }: { facts: DetailFact[] }) {
  const colors = useAppTheme();
  const populatedFacts = facts.filter((fact) => fact.values.length > 0);

  if (populatedFacts.length === 0) return null;

  return (
    <View style={[commonStyles.surfaceCard, styles.facts, { backgroundColor: colors.surfaceHigh }]}>
      {populatedFacts.map((fact, index) => (
        <View
          key={fact.label}
          style={[
            styles.factRow,
            index < populatedFacts.length - 1 && {
              borderBottomColor: colors.outline,
              borderBottomWidth: StyleSheet.hairlineWidth,
            },
          ]}
        >
          <Text style={[styles.factTitle, { color: colors.textMuted }]}>
            {fact.label.toUpperCase()}
          </Text>
          <Text selectable style={[styles.factValue, { color: colors.text }]}>
            {fact.values.join(" · ")}
          </Text>
        </View>
      ))}
    </View>
  );
}

export function FactGroup({ title, values }: { title: string; values: string[] }) {
  return <DetailFacts facts={[{ label: title, values }]} />;
}

export const detailStyles = {
  section: commonStyles.section,
  sectionTitle: commonStyles.sectionTitle,
  rows: { gap: 10 },
};

const styles = StyleSheet.create({
  content: {
    paddingBottom: 110,
    gap: spacing.xxxl,
  },
  header: {
    paddingHorizontal: PAGE_GUTTER,
    flexDirection: "row",
    alignItems: "flex-end",
    gap: spacing.xl,
  },
  cover: {
    width: 124,
    aspectRatio: 0.72,
    borderRadius: radius.md,
    borderCurve: "continuous",
  },
  heading: {
    flex: 1,
    minWidth: 0,
    paddingBottom: 4,
    gap: spacing.sm - 3,
  },
  eyebrow: {
    ...typography.detailEyebrow,
    letterSpacing: 1.2,
  },
  title: {
    ...typography.detailTitle,
  },
  summary: {
    paddingHorizontal: PAGE_GUTTER,
    ...typography.bodyLarge,
  },
  facts: {
    marginHorizontal: PAGE_GUTTER,
  },
  factRow: {
    paddingHorizontal: spacing.xl,
    paddingVertical: spacing.lg,
    gap: spacing.xxs + 1,
  },
  factTitle: {
    fontSize: 11,
    fontWeight: "800",
    letterSpacing: 0.8,
  },
  factValue: {
    fontSize: 15,
    lineHeight: 21,
    fontWeight: "600",
  },
});
