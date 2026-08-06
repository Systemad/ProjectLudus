import { Link, type Href } from "expo-router";
import { ChevronRight } from "lucide-react-native";
import { Pressable, Text, View } from "react-native";

import { GameCard } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { InlineState } from "@/shared/ui/inline-state";

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

  return (
    <View style={{ gap: 10 }}>
      <Link href={href} asChild>
        <Pressable
          accessibilityRole="button"
          accessibilityLabel={`View all ${title} games`}
          style={({ pressed }) => ({
            gap: 3,
            minHeight: 56,
            justifyContent: "center",
            opacity: pressed ? 0.68 : 1,
          })}
        >
          <View
            style={{ alignItems: "center", flexDirection: "row", justifyContent: "space-between" }}
          >
            <Text style={{ color: colors.text, fontSize: 22, fontWeight: "800", lineHeight: 28 }}>
              {title}
            </Text>
            <ChevronRight color={colors.primary} size={20} strokeWidth={2.4} />
          </View>
          <Text style={{ color: colors.textMuted, fontSize: 14 }}>{subtitle}</Text>
        </Pressable>
      </Link>

      {isLoading ? (
        <InlineState loading minHeight={180} />
      ) : isError ? (
        <InlineState
          minHeight={180}
          message="This list could not be loaded."
          onRetry={onRetry}
          retryLabel="Retry"
        />
      ) : games.length === 0 ? (
        <InlineState minHeight={180} message="No games are available in this list yet." />
      ) : (
        <View style={{ gap: 12 }}>
          {Array.from({ length: Math.ceil(Math.min(games.length, 6) / 2) }, (_, rowIndex) => {
            const rowGames = games.slice(rowIndex * 2, rowIndex * 2 + 2);
            return (
              <View key={`row-${rowIndex}`} style={{ flexDirection: "row", gap: 12 }}>
                {rowGames.map((game) => (
                  <View key={String(game.id)} style={{ flex: 1, minWidth: 0 }}>
                    <GameCard game={game} variant="grid" href={getGameHref(game)} />
                  </View>
                ))}
                {rowGames.length === 1 ? <View style={{ flex: 1, minWidth: 0 }} /> : null}
              </View>
            );
          })}
        </View>
      )}
    </View>
  );
}
