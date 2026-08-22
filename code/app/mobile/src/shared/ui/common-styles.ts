import { StyleSheet } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";

export const commonStyles = StyleSheet.create({
  pageGutter: {
    paddingHorizontal: PAGE_GUTTER,
  },
  section: {
    paddingHorizontal: PAGE_GUTTER,
    gap: 12,
  },
  sectionTitle: {
    fontSize: 22,
    fontWeight: "800",
  },
  centeredState: {
    alignItems: "center",
    justifyContent: "center",
    gap: 10,
  },
  fullScreenState: {
    flex: 1,
    minHeight: 240,
    padding: 24,
  },
  stateText: {
    fontSize: 14,
    textAlign: "center",
  },
  retry: {
    paddingHorizontal: 18,
    paddingVertical: 10,
    borderRadius: 22,
  },
  retryText: {
    fontWeight: "700",
  },
  surfaceCard: {
    overflow: "hidden",
    borderRadius: 16,
    borderCurve: "continuous",
  },
  row: {
    flexDirection: "row",
    alignItems: "center",
    gap: 12,
  },
});
