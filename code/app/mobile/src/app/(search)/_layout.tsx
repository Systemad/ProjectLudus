import { Stack } from "expo-router/stack";

import { CatalogStack, SettingsButton } from "@/navigation/catalog-stack";

export default function SearchLayout() {
  return (
    <CatalogStack>
      <Stack.Screen
        name="index"
        options={{
          title: "Search",
          headerRight: () => <SettingsButton href="/(search)/settings" />,
        }}
      />
      <Stack.Screen name="companies/[slug]" options={{ title: "Company" }} />
      <Stack.Screen name="events/[slug]" options={{ title: "Event" }} />
      <Stack.Screen name="settings" options={{ title: "Settings" }} />
    </CatalogStack>
  );
}
