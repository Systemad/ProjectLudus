import { useLocalSearchParams } from "expo-router";

import { GameDetail } from "@/features/game-detail";

export default function GameRoute() {
  const { slug } = useLocalSearchParams<{ slug: string }>();
  return <GameDetail slug={slug} />;
}
