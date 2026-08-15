import { Button, Column, Row, Spacer, Text } from "@expo/ui";
import { useWindowDimensions } from "react-native";

export type SteamSummaryProps = {
  currentPlayers?: number | null;
  peak24h?: number | null;
  steamAppId?: string | null;
  reviewDescription?: string | null;
  totalReviews?: number | null;
  finalCents?: number | null;
  currency?: string | null;
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
  currentPlayers,
  peak24h,
  steamAppId,
  reviewDescription,
  totalReviews,
  finalCents,
  currency,
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
        <Button disabled={!steamAppId} label="Open Steam" onPress={openSteam} variant="text" />
      </Row>
      <Row spacing={16}>
        <Stat
          label="Playing now"
          value={formatCount(currentPlayers)}
          width={statWidth}
          colors={colors}
        />
        <Stat label="24h peak" value={formatCount(peak24h)} width={statWidth} colors={colors} />
        <Stat label="Steam app ID" value={steamAppId ?? "—"} width={statWidth} colors={colors} />
      </Row>
      <Text textStyle={{ color: colors?.onSurface, fontSize: 20, fontWeight: "700" }}>
        Ratings and price
      </Text>
      <Row spacing={16}>
        <Stat
          label="Steam reviews"
          value={reviewDescription ?? "—"}
          width={statWidth}
          colors={colors}
        />
        <Stat
          label="Total reviews"
          value={formatCount(totalReviews)}
          width={statWidth}
          colors={colors}
        />
        <Stat
          label="Price"
          value={formatPrice(finalCents, currency)}
          width={statWidth}
          colors={colors}
        />
      </Row>
    </Column>
  );
}
