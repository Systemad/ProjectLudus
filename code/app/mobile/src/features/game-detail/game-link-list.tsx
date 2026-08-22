import * as Linking from "expo-linking";
import { ExternalLink } from "lucide-react-native";
import { Pressable, StyleSheet, Text, View } from "react-native";

import type { WebsiteDto } from "@/gen/types/WebsiteDto";
import { useAppTheme } from "@/hooks/use-app-theme";

export function GameLinkList({ websites }: { websites: WebsiteDto[] }) {
  const colors = useAppTheme();

  return (
    <View
      style={[styles.card, { backgroundColor: colors.surfaceHigh, borderColor: colors.outline }]}
    >
      {websites.map((website, index) => (
        <Pressable
          key={`${website.name}-${website.url}`}
          accessibilityRole="link"
          accessibilityLabel={`Open ${getWebsiteLabel(website.url, website.name)}`}
          onPress={() => void Linking.openURL(website.url)}
          style={({ pressed }) => [
            styles.row,
            index > 0 && {
              borderTopColor: colors.outline,
              borderTopWidth: StyleSheet.hairlineWidth,
            },
            { opacity: pressed ? 0.68 : 1 },
          ]}
        >
          <View style={styles.copy}>
            <Text style={[styles.title, { color: colors.text }]}>
              {getWebsiteLabel(website.url, website.name)}
            </Text>
            <Text numberOfLines={1} style={[styles.address, { color: colors.textMuted }]}>
              {getWebsiteAddress(website.url)}
            </Text>
          </View>
          <ExternalLink color={colors.primary} size={19} strokeWidth={2.3} />
        </Pressable>
      ))}
    </View>
  );
}

function getWebsiteLabel(url: string, name: string | null | undefined): string {
  const trimmedName = name?.trim();
  if (trimmedName && !looksLikeUrl(trimmedName)) return trimmedName;

  const address = url.toLowerCase();
  if (address.includes("steampowered.com")) return "Steam";
  if (address.includes("counter-strike.net")) return "Official website";
  if (address.includes("instagram.com")) return "Instagram";
  if (address.includes("youtube.com") || address.includes("youtu.be")) return "YouTube";
  return "Website";
}

function getWebsiteAddress(url: string): string {
  return url.replace(/^https?:\/\//, "").replace(/\/$/, "");
}

function looksLikeUrl(value: string): boolean {
  return /^https?:\/\//i.test(value) || value.includes("www.") || value.includes(".");
}

const styles = StyleSheet.create({
  card: {
    borderRadius: 16,
    borderWidth: StyleSheet.hairlineWidth,
    borderCurve: "continuous",
    overflow: "hidden",
  },
  row: {
    alignItems: "center",
    flexDirection: "row",
    gap: 12,
    minHeight: 68,
    paddingHorizontal: 16,
    paddingVertical: 10,
  },
  copy: {
    flex: 1,
    gap: 3,
    minWidth: 0,
  },
  title: {
    fontSize: 15,
    fontWeight: "800",
  },
  address: {
    fontSize: 12,
  },
});
