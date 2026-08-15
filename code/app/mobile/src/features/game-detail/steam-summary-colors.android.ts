import { useMaterialColors } from "@expo/ui/jetpack-compose";
import type { SteamSummaryColors } from "./steam-summary-content";

export function useSteamSummaryColors(): SteamSummaryColors {
  const colors = useMaterialColors();

  return {
    onSurface: colors.onSurface,
    onSurfaceVariant: colors.onSurfaceVariant,
  };
}
