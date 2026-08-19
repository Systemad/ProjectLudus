import { Link, type Href } from "expo-router";
import { Stack } from "expo-router/stack";
import { Settings } from "lucide-react-native";
import type { ReactNode } from "react";
import { Pressable, StyleSheet } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";

export function CatalogStack({ children }: { children: ReactNode }) {
  const colors = useAppTheme();

  return (
    <Stack
      screenOptions={({ route }) => ({
        headerStyle: { backgroundColor: colors.surface },
        headerTintColor: colors.text,
        contentStyle: { backgroundColor: colors.background },
        headerShadowVisible: false,
        ...(route.name === "games/[slug]" && {
          headerShown: false,
        }),
      })}
    >
      {children}
    </Stack>
  );
}

export function SettingsButton({ href }: { href: Href }) {
  const colors = useAppTheme();

  return (
    <Link href={href} asChild>
      <Pressable
        accessibilityRole="button"
        accessibilityLabel="Settings"
        hitSlop={8}
        style={({ pressed }) => [styles.settingsButton, { opacity: pressed ? 0.68 : 1 }]}
      >
        <Settings color={colors.text} size={22} strokeWidth={2.2} />
      </Pressable>
    </Link>
  );
}

const styles = StyleSheet.create({
  settingsButton: {
    width: 44,
    height: 44,
    alignItems: "center",
    justifyContent: "center",
  },
});
