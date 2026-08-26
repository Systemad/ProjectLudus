import type { TextStyle } from "react-native";

export const typography = {
  sectionTitle: {
    fontSize: 22,
    lineHeight: 28,
    fontWeight: "800",
  },
  heroTitle: {
    fontSize: 25,
    lineHeight: 30,
    fontWeight: "900",
  },
  detailTitle: {
    fontSize: 30,
    lineHeight: 34,
    fontWeight: "900",
  },
  detailSectionTitle: {
    fontSize: 19,
    fontWeight: "800",
  },
  detailEyebrow: {
    fontSize: 12,
    fontWeight: "900",
  },
  cardTitle: {
    fontSize: 15,
    lineHeight: 19,
    fontWeight: "700",
  },
  railTitle: {
    fontSize: 13,
    lineHeight: 16,
    fontWeight: "700",
  },
  cardMetadata: {
    fontSize: 12,
  },
  linkTitle: {
    fontSize: 15,
    fontWeight: "800",
  },
  bodyCompact: {
    fontSize: 14,
  },
  browseTitle: {
    fontSize: 16,
    fontWeight: "700",
  },
  browseInitial: {
    fontSize: 28,
    fontWeight: "800",
  },
  browseStatLabel: {
    fontSize: 10,
    fontWeight: "700",
  },
  browseStatValue: {
    fontSize: 16,
    fontWeight: "700",
  },
  metadataValue: {
    fontSize: 14,
    lineHeight: 19,
    fontWeight: "600",
  },
  placeholder: {
    fontSize: 32,
    fontWeight: "800",
  },
  bodyLarge: {
    fontSize: 16,
    lineHeight: 24,
  },
  microLabel: {
    fontSize: 11,
    fontWeight: "800",
  },
  body: {
    fontSize: 14,
    lineHeight: 20,
  },
  metadata: {
    fontSize: 12,
    lineHeight: 16,
  },
  label: {
    fontSize: 13,
    lineHeight: 18,
    fontWeight: "700",
  },
} satisfies Record<string, TextStyle>;
