import { Column, Row, Text, TextButton } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, padding } from "@expo/ui/jetpack-compose/modifiers";
import type { ReactNode } from "react";
import { Linking } from "react-native";

import type { GetPricingResponse } from "@/gen/types/GetPricingResponse";
import type { GetReviewsResponse } from "@/gen/types/GetReviewsResponse";
import type { SteamData } from "@/gen/types/SteamData";
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
  const steamUrl = steam?.steamAppId
    ? `https://store.steampowered.com/app/${steam.steamAppId}`
    : undefined;

  return (
    <Column
      modifiers={[fillMaxWidth(), padding(0, 14, 0, 0)]}
      verticalArrangement={{ spacedBy: 12 }}
    >
      <SectionHeader title="Steam now">
        <TextButton
          enabled={steamUrl !== undefined}
          onClick={() => steamUrl && void Linking.openURL(steamUrl)}
        >
          <Text>Open Steam</Text>
        </TextButton>
      </SectionHeader>
      <MetadataGrid
        columns={3}
        items={[
          { label: "Playing now", value: formatCount(steam?.currentPlayers) },
          { label: "24h peak", value: formatCount(steam?.peak24h) },
          { label: "Steam app ID", value: steam?.steamAppId ?? "—" },
        ]}
      />
      <Text style={{ typography: "titleLarge" }}>Ratings and price</Text>
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
    </Column>
  );
}

function SectionHeader({ title, children }: { title: string; children: ReactNode }) {
  return (
    <Row
      modifiers={[fillMaxWidth()]}
      horizontalArrangement="spaceBetween"
      verticalAlignment="center"
    >
      <Text style={{ typography: "titleLarge" }}>{title}</Text>
      {children}
    </Row>
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
