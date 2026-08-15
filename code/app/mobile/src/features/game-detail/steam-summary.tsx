import { Host } from "@expo/ui";
import { Linking } from "react-native";
import { SteamSummaryContent, type SteamSummaryProps } from "./steam-summary-content";
import { useSteamSummaryColors } from "./steam-summary-colors";

export function SteamSummary({
  currentPlayers,
  peak24h,
  steamAppId,
  reviewDescription,
  totalReviews,
  finalCents,
  currency,
}: SteamSummaryProps) {
  const colors = useSteamSummaryColors();

  return (
    <Host matchContents={{ vertical: true }} style={{ width: "100%" }}>
      <SteamSummaryContent
        currentPlayers={currentPlayers}
        peak24h={peak24h}
        steamAppId={steamAppId}
        reviewDescription={reviewDescription}
        totalReviews={totalReviews}
        finalCents={finalCents}
        currency={currency}
        colors={colors}
        openSteam={() => void Linking.openURL(`https://store.steampowered.com/app/${steamAppId}`)}
      />
    </Host>
  );
}
