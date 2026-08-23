import { Color } from "expo-router";
import { Platform, useColorScheme, type ColorValue } from "react-native";

export type AppTheme = {
  background: string;
  surface: string;
  surfaceHigh: string;
  primary: string;
  onPrimary: string;
  primaryContainer: string;
  onPrimaryContainer: string;
  text: string;
  textMuted: string;
  outline: string;
};

const fallback = {
  light: {
    background: "#FFFBFE",
    surface: "#FFFBFE",
    surfaceHigh: "#ECE6F0",
    primary: "#6750A4",
    onPrimary: "#FFFFFF",
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
    onPrimary: "#381E72",
    primaryContainer: "#4F378B",
    onPrimaryContainer: "#EADDFF",
    text: "#E6E1E5",
    textMuted: "#CAC4D0",
    outline: "#938F99",
  },
} satisfies Record<"light" | "dark", AppTheme>;

export function useAppTheme(): AppTheme {
  const scheme = useColorScheme() === "dark" ? "dark" : "light";
  const palette = fallback[scheme];

  return {
    ...palette,
    background: nativeColor(
      Color.ios.systemBackground,
      Color.android.dynamic.surface,
      Color.android.material.surface,
      palette.background,
    ),
    surface: nativeColor(
      Color.ios.systemBackground,
      Color.android.dynamic.surface,
      Color.android.material.surface,
      palette.surface,
    ),
    surfaceHigh: nativeColor(
      Color.ios.secondarySystemBackground,
      Color.android.dynamic.surfaceContainerHigh,
      Color.android.material.surfaceContainerHigh,
      palette.surfaceHigh,
    ),
    primary: nativeColor(
      Color.ios.systemBlue,
      Color.android.dynamic.primary,
      Color.android.material.primary,
      palette.primary,
    ),
    onPrimary: nativeColor(
      Color.ios.systemBackground,
      Color.android.dynamic.onPrimary,
      Color.android.material.onPrimary,
      palette.onPrimary,
    ),
    primaryContainer: nativeColor(
      Color.ios.systemGray5,
      Color.android.dynamic.primaryContainer,
      Color.android.material.primaryContainer,
      palette.primaryContainer,
    ),
    onPrimaryContainer: nativeColor(
      Color.ios.label,
      Color.android.dynamic.onPrimaryContainer,
      Color.android.material.onPrimaryContainer,
      palette.onPrimaryContainer,
    ),
    text: nativeColor(
      Color.ios.label,
      Color.android.dynamic.onSurface,
      Color.android.material.onSurface,
      palette.text,
    ),
    textMuted: nativeColor(
      Color.ios.secondaryLabel,
      Color.android.dynamic.onSurfaceVariant,
      Color.android.material.onSurfaceVariant,
      palette.textMuted,
    ),
    outline: nativeColor(
      Color.ios.separator,
      Color.android.dynamic.outlineVariant,
      Color.android.material.outlineVariant,
      palette.outline,
    ),
  };
}

function nativeColor(
  ios: ColorValue,
  androidDynamic: ColorValue,
  androidStatic: ColorValue,
  fallbackColor: string,
): string {
  const android = androidDynamic ?? androidStatic;
  // SAFETY: Platform.select returns a platform color or the checked string fallback.
  return (Platform.select({ ios, android, default: fallbackColor }) ?? fallbackColor) as string;
}
