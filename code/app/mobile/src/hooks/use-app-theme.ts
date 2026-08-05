import { Color } from "expo-router";
import { Platform, useColorScheme, type ColorValue } from "react-native";

export type AppTheme = {
  background: ColorValue;
  surface: ColorValue;
  surfaceHigh: ColorValue;
  primary: ColorValue;
  primaryContainer: ColorValue;
  onPrimaryContainer: ColorValue;
  text: ColorValue;
  textMuted: ColorValue;
  outline: ColorValue;
};

const fallback = {
  light: {
    background: "#FFFBFE",
    surface: "#FFFBFE",
    surfaceHigh: "#ECE6F0",
    primary: "#6750A4",
    primaryContainer: "#EADDFF",
    onPrimaryContainer: "#21005D",
    text: "#1D1B20",
    textMuted: "#49454F",
    outline: "#79747E",
  },
  dark: {
    background: "#141218",
    surface: "#141218",
    surfaceHigh: "#2B2930",
    primary: "#D0BCFF",
    primaryContainer: "#4F378B",
    onPrimaryContainer: "#EADDFF",
    text: "#E6E1E5",
    textMuted: "#CAC4D0",
    outline: "#938F99",
  },
} satisfies Record<"light" | "dark", AppTheme>;

export function useAppTheme(): AppTheme {
  const scheme = useColorScheme() === "dark" ? "dark" : "light";
  if (Platform.OS !== "android") return fallback[scheme];

  return {
    background: Color.android.dynamic.background,
    surface: Color.android.dynamic.surface,
    surfaceHigh: Color.android.dynamic.surfaceContainerHigh,
    primary: Color.android.dynamic.primary,
    primaryContainer: Color.android.dynamic.primaryContainer,
    onPrimaryContainer: Color.android.dynamic.onPrimaryContainer,
    text: Color.android.dynamic.onSurface,
    textMuted: Color.android.dynamic.onSurfaceVariant,
    outline: Color.android.dynamic.outlineVariant,
  };
}
