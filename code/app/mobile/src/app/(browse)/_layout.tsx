import { Stack } from "expo-router/stack";

import { CatalogStack } from "@/navigation/catalog-stack";

export default function BrowseLayout() {
  return (
    <CatalogStack>
      <Stack.Screen
        name="index"
        options={{
          title: "Browse",
        }}
      />
      <Stack.Screen name="games/[slug]" options={{ headerShown: false }} />
      <Stack.Screen name="companies/[slug]" options={{ title: "Company" }} />
      <Stack.Screen name="events/[slug]" options={{ title: "Event" }} />
      <Stack.Screen name="settings" options={{ title: "Settings" }} />
    </CatalogStack>
  );
}
