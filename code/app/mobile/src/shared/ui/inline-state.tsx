import { ActivityIndicator, Pressable, Text, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";
import { commonStyles } from "./common-styles";

type InlineStateProps = {
  loading?: boolean;
  title?: string;
  message?: string;
  onRetry?: () => void;
  retryLabel?: string;
  minHeight?: number;
};

export function InlineState({
  loading = false,
  title,
  message,
  onRetry,
  retryLabel = "Try again",
  minHeight = 120,
}: InlineStateProps) {
  const colors = useAppTheme();

  return (
    <View style={[commonStyles.centeredState, { minHeight }]}>
      {loading ? <ActivityIndicator color={colors.primary} /> : null}
      {title ? (
        <Text style={{ color: colors.text, fontSize: 18, fontWeight: "800" }}>{title}</Text>
      ) : null}
      {message ? (
        <Text style={[commonStyles.stateText, { color: colors.textMuted }]}>{message}</Text>
      ) : null}
      {onRetry ? (
        <Pressable
          accessibilityRole="button"
          onPress={onRetry}
          style={[commonStyles.retry, { backgroundColor: colors.primaryContainer }]}
        >
          <Text style={[commonStyles.retryText, { color: colors.onPrimaryContainer }]}>
            {retryLabel}
          </Text>
        </Pressable>
      ) : null}
    </View>
  );
}
