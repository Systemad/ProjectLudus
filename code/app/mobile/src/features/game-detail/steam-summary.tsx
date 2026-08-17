import { Host } from "@expo/ui";
import { Linking } from "react-native";
import { SteamSummaryContent, type SteamSummaryProps } from "./steam-summary-content";
import { useSteamSummaryColors } from "./steam-summary-colors";

export function SteamSummary({ steam, reviews, pricing }: SteamSummaryProps) {
  const colors = useSteamSummaryColors();

  return (
    <Host matchContents={{ vertical: true }} style={{ width: "100%" }}>
      <SteamSummaryContent
        steam={steam}
        reviews={reviews}
        pricing={pricing}
        colors={colors}
        openSteam={() =>
          steam?.steamAppId
            ? void Linking.openURL(`https://store.steampowered.com/app/${steam.steamAppId}`)
            : undefined
        }
      />
    </Host>
  );
}
