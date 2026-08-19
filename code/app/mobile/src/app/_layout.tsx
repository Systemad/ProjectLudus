import { QueryClientProvider } from "@tanstack/react-query";
import { DarkTheme, DefaultTheme, ThemeProvider } from "expo-router/react-navigation";
import { NativeTabs } from "expo-router/unstable-native-tabs";
import { useColorScheme } from "react-native";
import { SafeAreaProvider } from "react-native-safe-area-context";

import { LastVisitedProvider } from "@/features/last-visited";
import { AuthProvider } from "@/features/profile";
import { useAppTheme } from "@/hooks/use-app-theme";
import { queryClient } from "@/lib/query-client";
import "@/api/api-client";
import "@/api/play-api-client";

export default function RootLayout() {
  const scheme = useColorScheme();
  const colors = useAppTheme();

  return (
    <SafeAreaProvider>
      <QueryClientProvider client={queryClient}>
        <LastVisitedProvider>
          <AuthProvider>
            <ThemeProvider value={scheme === "dark" ? DarkTheme : DefaultTheme}>
              <NativeTabs
                backgroundColor={colors.surface}
                tintColor={colors.primary}
                labelStyle={{ color: colors.text }}
              >
                <NativeTabs.Trigger name="(discover)">
                  <NativeTabs.Trigger.Icon sf="safari.fill" md="explore" />
                  <NativeTabs.Trigger.Label>Discover</NativeTabs.Trigger.Label>
                </NativeTabs.Trigger>
                <NativeTabs.Trigger name="(browse)">
                  <NativeTabs.Trigger.Icon sf="square.grid.2x2.fill" md="grid_view" />
                  <NativeTabs.Trigger.Label>Browse</NativeTabs.Trigger.Label>
                </NativeTabs.Trigger>
                <NativeTabs.Trigger name="(search)">
                  <NativeTabs.Trigger.Icon sf="magnifyingglass" md="search" />
                  <NativeTabs.Trigger.Label>Search</NativeTabs.Trigger.Label>
                </NativeTabs.Trigger>
                <NativeTabs.Trigger name="profile">
                  <NativeTabs.Trigger.Icon sf="person.crop.circle" md="account_circle" />
                  <NativeTabs.Trigger.Label>Profile</NativeTabs.Trigger.Label>
                </NativeTabs.Trigger>
              </NativeTabs>
            </ThemeProvider>
          </AuthProvider>
        </LastVisitedProvider>
      </QueryClientProvider>
    </SafeAreaProvider>
  );
}
