import { AssistChip, Row, Text } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, horizontalScroll } from "@expo/ui/jetpack-compose/modifiers";
import type { Href } from "expo-router";
import { useRouter } from "expo-router";

import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";

export function RelatedGameRail({
  games,
  getHref,
}: {
  games: GameBrowseDto[];
  getHref: (gameId: string) => Href;
}) {
  const router = useRouter();

  return (
    <Row modifiers={[fillMaxWidth(), horizontalScroll()]} horizontalArrangement={{ spacedBy: 8 }}>
      {games.map((game) => (
        <AssistChip key={game.id} onClick={() => router.push(getHref(game.id))}>
          <AssistChip.Label>
            <Text>{game.name}</Text>
          </AssistChip.Label>
        </AssistChip>
      ))}
    </Row>
  );
}
