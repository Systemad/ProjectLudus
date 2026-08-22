import * as Linking from "expo-linking";
import { Pressable, StyleSheet, Text, View } from "react-native";

import type { GetPricingResponse } from "@/gen/types/GetPricingResponse";
import type { GetReviewsResponse } from "@/gen/types/GetReviewsResponse";
import type { SteamData } from "@/gen/types/SteamData";
import { useAppTheme } from "@/hooks/use-app-theme";
import { MetadataGrid } from "@/shared/ui/metadata-grid";

export function SteamSummary({
  steam,
  reviews,
  pricing,
}: {
  steam?: SteamData | null;
  reviews?: GetReviewsResponse;
  pricing?: GetPricingResponse;
}) {
  const colors = useAppTheme();
  const steamUrl = steam?.steamAppId
    ? `https://store.steampowered.com/app/${steam.steamAppId}`
    : undefined;

  return (
    <View style={styles.section}>
      <View style={styles.header}>
        <Text style={[styles.title, { color: colors.text }]}>Steam now</Text>
        <Pressable
          accessibilityRole="link"
          accessibilityLabel="Open this game on Steam"
          disabled={!steamUrl}
          onPress={() => steamUrl && void Linking.openURL(steamUrl)}
          style={({ pressed }) => [styles.link, { opacity: !steamUrl || pressed ? 0.5 : 1 }]}
        >
          <Text style={[styles.linkText, { color: colors.primary }]}>Open Steam</Text>
        </Pressable>
      </View>
      <MetadataGrid
        columns={3}
        items={[
          { label: "Playing now", value: formatCount(steam?.currentPlayers) },
          { label: "24h peak", value: formatCount(steam?.peak24h) },
          { label: "Steam app ID", value: steam?.steamAppId ?? "—" },
        ]}
      />
      <Text style={[styles.title, { color: colors.text }]}>Ratings and price</Text>
      <MetadataGrid
        columns={2}
        items={[
          { label: "Steam reviews", value: formatReviewScore(reviews?.reviewScore) },
          {
            label: "Total reviews",
            value: formatCount(reviews?.totalReviews ?? steam?.review?.totalReviews),
          },
          {
            label: "Price",
            value: formatPrice(
              pricing?.finalCents ?? steam?.pricing?.finalCents,
              pricing?.currency,
            ),
          },
          { label: "IGDB rating", value: "—" },
        ]}
      />
    </View>
  );
}

function formatCount(value: number | null | undefined) {
  return value === null || value === undefined ? "—" : value.toLocaleString();
}

function formatReviewScore(value: number | null | undefined) {
  return value === null || value === undefined ? "—" : `${value}/10`;
}

function formatPrice(finalCents: number | null | undefined, currency: string | null | undefined) {
  if (finalCents === null || finalCents === undefined) return "—";
  if (finalCents === 0) return "Free to play";
  return `${currency ?? ""} ${(finalCents / 100).toFixed(2)}`.trim();
}

const styles = StyleSheet.create({
  section: {
    gap: 10,
  },
  header: {
    alignItems: "center",
    flexDirection: "row",
    justifyContent: "space-between",
  },
  title: {
    fontSize: 19,
    fontWeight: "800",
  },
  link: {
    minHeight: 36,
    justifyContent: "center",
    paddingHorizontal: 4,
  },
  linkText: {
    fontSize: 14,
    fontWeight: "800",
  },
});
