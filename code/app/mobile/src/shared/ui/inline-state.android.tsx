import { Host } from "@expo/ui";
import { Button, Column, LoadingIndicator, Text } from "@expo/ui/jetpack-compose";
import { fillMaxSize, fillMaxWidth, paddingAll } from "@expo/ui/jetpack-compose/modifiers";

import { useAppTheme } from "@/hooks/use-app-theme";

import type { InlineStateProps } from "./inline-state.types";

export type { InlineStateProps } from "./inline-state.types";

export function InlineState({
  loading = false,
  title,
  message,
  onRetry,
  retryLabel = "Try again",
  minHeight = 120,
  fullScreen = false,
}: InlineStateProps) {
  const colors = useAppTheme();

  return (
    <Host
      style={
        fullScreen
          ? { flex: 1, backgroundColor: colors.background }
          : { minHeight, width: "100%", backgroundColor: colors.background }
      }
    >
      <Column
        horizontalAlignment="center"
        verticalAlignment="center"
        verticalArrangement={{ spacedBy: 10 }}
        modifiers={[fullScreen ? fillMaxSize() : fillMaxWidth(), paddingAll(24)]}
      >
        {loading ? <LoadingIndicator color={colors.primary} /> : null}
        {title ? (
          <Text style={{ textAlign: "center", typography: "titleLarge" }}>{title}</Text>
        ) : null}
        {message ? (
          <Text style={{ textAlign: "center", typography: "bodyMedium" }}>{message}</Text>
        ) : null}
        {onRetry ? (
          <Button onClick={onRetry}>
            <Text>{retryLabel}</Text>
          </Button>
        ) : null}
      </Column>
    </Host>
  );
}
