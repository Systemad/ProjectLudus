import { QueryClientProvider } from "@tanstack/react-query";
import { DarkTheme, DefaultTheme, ThemeProvider } from "expo-router/react-navigation";
import { Stack } from "expo-router/stack";
import { useColorScheme } from "react-native";
import { SafeAreaProvider } from "react-native-safe-area-context";

import { LastVisitedProvider } from "@/features/last-visited";
import { AuthProvider } from "@/features/profile";
import { queryClient } from "@/lib/query-client";
import "@/api/api-client";
import "@/api/play-api-client";

export default function RootLayout() {
  const scheme = useColorScheme();

  return (
    <SafeAreaProvider>
      <QueryClientProvider client={queryClient}>
        <LastVisitedProvider>
          <AuthProvider>
            <ThemeProvider value={scheme === "dark" ? DarkTheme : DefaultTheme}>
              <Stack screenOptions={{ headerShown: false }}>
                <Stack.Screen name="(tabs)" />
                <Stack.Screen name="games/[slug]" options={{ headerShown: true }} />
                <Stack.Screen name="settings" options={{ headerShown: true, title: "Settings" }} />
              </Stack>
            </ThemeProvider>
          </AuthProvider>
        </LastVisitedProvider>
      </QueryClientProvider>
    </SafeAreaProvider>
  );
}
