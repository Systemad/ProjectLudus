import { useMaterialColors } from "@expo/ui/jetpack-compose";

import type { AppTheme } from "./use-app-theme";

export function useAppTheme(): AppTheme {
  const colors = useMaterialColors();

  return {
    background: colors.background,
    surface: colors.surface,
    surfaceHigh: colors.surfaceContainerHigh,
    primary: colors.primary,
    onPrimary: colors.onPrimary,
    primaryContainer: colors.primaryContainer,
    onPrimaryContainer: colors.onPrimaryContainer,
    text: colors.onSurface,
    textMuted: colors.onSurfaceVariant,
    outline: colors.outlineVariant,
  };
}
