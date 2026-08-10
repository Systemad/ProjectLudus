import { Host } from "@expo/ui";
import { Card, Column, Icon, ListItem, Text } from "@expo/ui/jetpack-compose";
import {
  clickable,
  fillMaxSize,
  fillMaxWidth,
  paddingAll,
  verticalScroll,
} from "@expo/ui/jetpack-compose/modifiers";
import { Linking } from "react-native";

import { PAGE_GUTTER } from "@/config/layout";

const githubUrl = "https://github.com/example/game-index" as const;

export function SettingsScreen() {
  return (
    <Host style={{ flex: 1 }}>
      <Column modifiers={[fillMaxSize(), verticalScroll(), paddingAll(PAGE_GUTTER)]}>
        <Column verticalArrangement={{ spacedBy: 10 }} modifiers={[fillMaxWidth()]}>
          <Text style={{ typography: "labelLarge" }}>About</Text>
          <Card modifiers={[fillMaxWidth(), clickable(() => void Linking.openURL(githubUrl))]}>
            <ListItem>
              <ListItem.LeadingContent>
                <Icon source={require("@/assets/icons/github.xml")} contentDescription="GitHub" />
              </ListItem.LeadingContent>
              <ListItem.HeadlineContent>
                <Text>GitHub</Text>
              </ListItem.HeadlineContent>
              <ListItem.SupportingContent>
                <Text>Placeholder repository</Text>
              </ListItem.SupportingContent>
            </ListItem>
          </Card>
        </Column>
      </Column>
    </Host>
  );
}
