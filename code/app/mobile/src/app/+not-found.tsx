import { Button, Column, Host, Text } from "@expo/ui";
import { Link } from "expo-router";

import { useAppTheme } from "@/hooks/use-app-theme";

export default function NotFoundScreen() {
  const colors = useAppTheme();

  return (
    <Host style={{ flex: 1, backgroundColor: colors.background }}>
      <Column alignment="center" spacing={12} style={{ padding: 24 }}>
        <Text textStyle={{ fontSize: 28, fontWeight: "900" }}>Page not found</Text>
        <Text textStyle={{ fontSize: 16, textAlign: "center" }}>
          The catalog does not have a page at this address.
        </Text>
        <Link href="/" asChild>
          <Button label="Back to Discover" />
        </Link>
      </Column>
    </Host>
  );
}
