import { Column, Host, ListItem, ScrollView, Text } from "@expo/ui";
import { Github } from "lucide-react-native";
import { Linking } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";

const githubUrl = "https://github.com/example/game-index" as const;
export function SettingsScreen() {
  return (
    <Host style={{ flex: 1 }}>
      <ScrollView>
        <Column spacing={28} style={{ paddingHorizontal: PAGE_GUTTER, paddingBottom: 32 }}>
          <Column spacing={10}>
            <Text textStyle={{ fontSize: 13, fontWeight: "800", letterSpacing: 0.8 }}>About</Text>
            <ListItem
              leading={<Github size={22} strokeWidth={2.2} />}
              onPress={() => void Linking.openURL(githubUrl)}
              supportingText="Placeholder repository"
            >
              GitHub
            </ListItem>
          </Column>
        </Column>
      </ScrollView>
    </Host>
  );
}
