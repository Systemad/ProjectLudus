import { Stack } from "expo-router/stack";

import { CatalogStack, SettingsButton } from "@/navigation/catalog-stack";

export default function DiscoverLayout() {
  return (
    <CatalogStack>
      <Stack.Screen
        name="index"
        options={{
          title: "Discover",
          headerRight: () => <SettingsButton href="/(discover)/settings" />,
        }}
      />
      <Stack.Screen name="companies/[slug]" options={{ title: "Company" }} />
      <Stack.Screen name="events/[slug]" options={{ title: "Event" }} />
      <Stack.Screen name="collections/[collection]" options={{ title: "Collection" }} />
      <Stack.Screen name="settings" options={{ title: "Settings" }} />
    </CatalogStack>
  );
}
