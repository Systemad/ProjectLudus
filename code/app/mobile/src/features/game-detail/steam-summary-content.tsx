import { Button, Column, Row, Spacer, Text } from "@expo/ui";
import { useWindowDimensions } from "react-native";
import type { GetPricingResponse } from "@/gen/types/GetPricingResponse";
import type { GetReviewsResponse } from "@/gen/types/GetReviewsResponse";
import type { SteamData } from "@/gen/types/SteamData";

export type SteamSummaryProps = {
  steam?: SteamData | null;
  reviews?: GetReviewsResponse;
  pricing?: GetPricingResponse;
};

export type SteamSummaryColors = {
  onSurface: string;
  onSurfaceVariant: string;
};

function formatCount(value: number | null | undefined) {
  return value === null || value === undefined ? "—" : value.toLocaleString();
}

function formatPrice(finalCents: number | null | undefined, currency: string | null | undefined) {
  if (finalCents === null || finalCents === undefined) return "—";
  if (finalCents === 0) return "Free to play";
  return `${currency ?? ""} ${(finalCents / 100).toFixed(2)}`.trim();
}

function Stat({
  label,
  value,
  width,
  colors,
}: {
  label: string;
  value: string;
  width: number;
  colors?: SteamSummaryColors;
}) {
  return (
    <Column spacing={3} style={{ width }}>
      <Text textStyle={{ color: colors?.onSurface, fontSize: 18, fontWeight: "800" }}>{value}</Text>
      <Text textStyle={{ color: colors?.onSurfaceVariant, fontSize: 12 }}>{label}</Text>
    </Column>
  );
}

export function SteamSummaryContent({
  steam,
  reviews,
  pricing,
  openSteam,
  colors,
}: SteamSummaryProps & { colors?: SteamSummaryColors; openSteam: () => void }) {
  const { width } = useWindowDimensions();
  const statWidth = (width - 64) / 3;

  return (
    <Column spacing={14} style={{ paddingHorizontal: 16 }}>
      <Row alignment="center">
        <Text textStyle={{ color: colors?.onSurface, fontSize: 20, fontWeight: "700" }}>
          Steam now
        </Text>
        <Spacer flexible />
        <Button
          disabled={!steam?.steamAppId}
          label="Open Steam"
          onPress={openSteam}
          variant="text"
        />
      </Row>
      <Row spacing={16}>
        <Stat
          label="Playing now"
          value={formatCount(steam?.currentPlayers)}
          width={statWidth}
          colors={colors}
        />
        <Stat
          label="24h peak"
          value={formatCount(steam?.peak24h)}
          width={statWidth}
          colors={colors}
        />
        <Stat
          label="Steam app ID"
          value={steam?.steamAppId ?? "—"}
          width={statWidth}
          colors={colors}
        />
      </Row>
      <Text textStyle={{ color: colors?.onSurface, fontSize: 20, fontWeight: "700" }}>
        Ratings and price
      </Text>
      <Row spacing={16}>
        <Stat
          label="Steam reviews"
          value={reviews?.reviewScoreDesc ?? "—"}
          width={statWidth}
          colors={colors}
        />
        <Stat
          label="Total reviews"
          value={formatCount(reviews?.totalReviews)}
          width={statWidth}
          colors={colors}
        />
        <Stat
          label="Price"
          value={formatPrice(pricing?.finalCents, pricing?.currency)}
          width={statWidth}
          colors={colors}
        />
      </Row>
    </Column>
  );
}
