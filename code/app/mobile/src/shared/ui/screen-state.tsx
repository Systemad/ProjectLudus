import { ActivityIndicator } from "react-native";

import { Button, Column, Host, RNHostView, Text } from "@expo/ui";

import { useAppTheme } from "@/hooks/use-app-theme";

function StateLayout({ children }: { children: React.ReactNode }) {
  const colors = useAppTheme();

  return (
    <Host style={{ flex: 1, backgroundColor: colors.background }}>
      <Column alignment="center" spacing={12} style={{ padding: 24 }}>
        {children}
      </Column>
    </Host>
  );
}

export function LoadingState({ label = "Loading catalog…" }: { label?: string }) {
  return (
    <StateLayout>
      <RNHostView matchContents>
        <ActivityIndicator />
      </RNHostView>
      <Text textStyle={{ fontSize: 15 }}>{label}</Text>
    </StateLayout>
  );
}

export function EmptyState({ title, message }: { title: string; message: string }) {
  return (
    <StateLayout>
      <Text textStyle={{ fontSize: 20, fontWeight: "700" }}>{title}</Text>
      <Text textStyle={{ fontSize: 15 }}>{message}</Text>
    </StateLayout>
  );
}

export function ErrorState({ onRetry }: { onRetry: () => void }) {
  const colors = useAppTheme();

  return (
    <StateLayout>
      <Text textStyle={{ fontSize: 20, fontWeight: "700" }}>Couldn’t load this page</Text>
      <Text textStyle={{ fontSize: 15 }}>The Game-index API could not be reached.</Text>
      <Button
        label="Try again"
        onPress={onRetry}
        style={{ backgroundColor: colors.primaryContainer, borderRadius: 24, padding: 12 }}
      />
    </StateLayout>
  );
}
