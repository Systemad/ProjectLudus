import { StyleSheet } from "react-native";

import { CONTENT_STATE_MIN_HEIGHT, PAGE_GUTTER } from "@/config/layout";
import { radius, spacing, typography } from "@/theme";

export const commonStyles = StyleSheet.create({
  pageGutter: {
    paddingHorizontal: PAGE_GUTTER,
  },
  section: {
    paddingHorizontal: PAGE_GUTTER,
    gap: spacing.md,
  },
  sectionTitle: typography.sectionTitle,
  centeredState: {
    alignItems: "center",
    justifyContent: "center",
    gap: spacing.sm,
  },
  fullScreenState: {
    flex: 1,
    minHeight: CONTENT_STATE_MIN_HEIGHT,
    padding: spacing.xxxl,
  },
  stateText: { ...typography.body, textAlign: "center" },
  retry: {
    paddingHorizontal: spacing.xxl,
    paddingVertical: spacing.sm,
    borderRadius: radius.xl,
  },
  retryText: {
    fontWeight: "700",
  },
  surfaceCard: {
    overflow: "hidden",
    borderRadius: radius.lg,
    borderCurve: "continuous",
  },
  row: {
    flexDirection: "row",
    alignItems: "center",
    gap: spacing.md,
  },
});
