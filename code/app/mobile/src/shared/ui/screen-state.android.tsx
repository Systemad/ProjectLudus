import { Host } from "@expo/ui";
import { Button, Column, LoadingIndicator, Text } from "@expo/ui/jetpack-compose";
import { fillMaxSize, paddingAll } from "@expo/ui/jetpack-compose/modifiers";

function StateLayout({ children }: { children: React.ReactNode }) {
  return (
    <Host style={{ flex: 1 }}>
      <Column
        horizontalAlignment="center"
        verticalArrangement={{ spacedBy: 12 }}
        verticalAlignment="center"
        modifiers={[fillMaxSize(), paddingAll(24)]}
      >
        {children}
      </Column>
    </Host>
  );
}

export function LoadingState({ label = "Loading catalog…" }: { label?: string }) {
  return (
    <StateLayout>
      <LoadingIndicator />
      <Text style={{ textAlign: "center", typography: "bodyMedium" }}>{label}</Text>
    </StateLayout>
  );
}

export function EmptyState({ title, message }: { title: string; message: string }) {
  return (
    <StateLayout>
      <Text style={{ textAlign: "center", typography: "titleLarge" }}>{title}</Text>
      <Text style={{ textAlign: "center", typography: "bodyMedium" }}>{message}</Text>
    </StateLayout>
  );
}

export function ErrorState({ onRetry }: { onRetry: () => void }) {
  return (
    <StateLayout>
      <Text style={{ textAlign: "center", typography: "titleLarge" }}>Couldn’t load this page</Text>
      <Text style={{ textAlign: "center", typography: "bodyMedium" }}>
        The Game-index API could not be reached.
      </Text>
      <Button onClick={onRetry}>
        <Text>Try again</Text>
      </Button>
    </StateLayout>
  );
}
