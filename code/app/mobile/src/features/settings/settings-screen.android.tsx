import { Host } from "@expo/ui";
import { Card, Column, ListItem, RNHostView, Text } from "@expo/ui/jetpack-compose";
import {
  clickable,
  fillMaxSize,
  fillMaxWidth,
  paddingAll,
  verticalScroll,
} from "@expo/ui/jetpack-compose/modifiers";
import { Linking } from "react-native";
import { Github } from "lucide-react-native";

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
                <RNHostView matchContents>
                  <Github size={22} strokeWidth={2.2} />
                </RNHostView>
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
