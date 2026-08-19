import { Card, Column, ListItem, Text } from "@expo/ui/jetpack-compose";
import { clickable, fillMaxWidth } from "@expo/ui/jetpack-compose/modifiers";
import * as Linking from "expo-linking";

import type { WebsiteDto } from "@/gen/types/WebsiteDto";

export function GameLinkList({ websites }: { websites: WebsiteDto[] }) {
  return (
    <Card modifiers={[fillMaxWidth()]}>
      <Column modifiers={[fillMaxWidth()]}>
        {websites.map((website) => (
          <ListItem
            key={`${website.name}-${website.url}`}
            modifiers={[clickable(() => void Linking.openURL(website.url))]}
          >
            <ListItem.HeadlineContent>
              <Text style={{ typography: "titleSmall" }}>{website.name || "Website"}</Text>
            </ListItem.HeadlineContent>
            <ListItem.SupportingContent>
              <Text style={{ typography: "bodySmall" }}>{website.url}</Text>
            </ListItem.SupportingContent>
          </ListItem>
        ))}
      </Column>
    </Card>
  );
}
