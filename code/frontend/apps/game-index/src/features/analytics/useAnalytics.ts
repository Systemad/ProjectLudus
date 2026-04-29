import * as CookieConsent from "vanilla-cookieconsent";
import { useGamesAnalyticsRecordGameEventHook } from "@src/gen/playApi";

export function useAnalytics() {
  const mutation = useGamesAnalyticsRecordGameEventHook();

  const track = (gameId: number, eventType: "View" | "Click") => {
    if (!CookieConsent.acceptedCategory("analytics")) return;
    mutation.mutate({ data: { gameId, eventType } });
  };

  return { track };
}
