import { Host } from "@expo/ui";
import { Button, Column, RNHostView, Row, Spacer, Text } from "@expo/ui/jetpack-compose";
import {
  clickable,
  fillMaxWidth,
  padding,
  paddingAll,
  weight,
} from "@expo/ui/jetpack-compose/modifiers";
import { useRouter, type Href } from "expo-router";

import { GameCard } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";

type BrowseSectionProps = {
  title: string;
  subtitle: string;
  href: Href;
  games: GameBrowseDto[];
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
  getGameHref: (game: GameBrowseDto) => Href;
};

export function BrowseSection({
  title,
  subtitle,
  href,
  games,
  isLoading,
  isError,
  onRetry,
  getGameHref,
}: BrowseSectionProps) {
  const colors = useAppTheme();
  const router = useRouter();
  const visibleGames = games.slice(0, 6);
  const rows = Array.from({ length: Math.ceil(visibleGames.length / 2) }, (_, index) =>
    visibleGames.slice(index * 2, index * 2 + 2),
  );

  return (
    <Host matchContents style={{ width: "100%" }}>
      <Column modifiers={[fillMaxWidth(), paddingAll(4)]} verticalArrangement={{ spacedBy: 10 }}>
        <Row
          modifiers={[fillMaxWidth(), clickable(() => router.push(href))]}
          verticalAlignment="center"
        >
          <Column modifiers={[weight(1)]} verticalArrangement={{ spacedBy: 2 }}>
            <Text color={colors.text} style={{ typography: "headlineSmall", fontWeight: "800" }}>
              {title}
            </Text>
            <Text color={colors.textMuted} style={{ typography: "bodyMedium" }}>
              {subtitle}
            </Text>
          </Column>
          <Text color={colors.primary} style={{ typography: "headlineSmall", fontWeight: "800" }}>
            ›
          </Text>
        </Row>

        {isLoading ? (
          <Text color={colors.textMuted} style={{ typography: "bodyMedium" }}>
            Loading games…
          </Text>
        ) : isError ? (
          <Column
            modifiers={[fillMaxWidth(), padding(0, 8, 0, 8)]}
            verticalArrangement={{ spacedBy: 8 }}
          >
            <Text color={colors.textMuted} style={{ typography: "bodyMedium" }}>
              This list could not be loaded.
            </Text>
            <Button onClick={onRetry}>
              <Text>Retry</Text>
            </Button>
          </Column>
        ) : rows.length === 0 ? (
          <Text color={colors.textMuted} style={{ typography: "bodyMedium" }}>
            No games are available in this list yet.
          </Text>
        ) : (
          <Column modifiers={[fillMaxWidth()]} verticalArrangement={{ spacedBy: 12 }}>
            {rows.map((row, rowIndex) => (
              <Row
                key={`row-${rowIndex}`}
                modifiers={[fillMaxWidth()]}
                horizontalArrangement={{ spacedBy: 12 }}
              >
                {row.map((game) => (
                  <RNHostView key={String(game.id)} modifiers={[weight(1)]}>
                    <GameCard game={game} variant="grid" href={getGameHref(game)} />
                  </RNHostView>
                ))}
                {row.length === 1 ? <Spacer modifiers={[weight(1)]} /> : null}
              </Row>
            ))}
          </Column>
        )}
      </Column>
    </Host>
  );
}
