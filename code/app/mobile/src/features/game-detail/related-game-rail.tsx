import { Image } from "expo-image";
import type { Href } from "expo-router";
import { useRouter } from "expo-router";
import { Pressable, ScrollView, StyleSheet, Text, View } from "react-native";

import { getGameCardImage } from "@/entities/game/game-image";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import { useAppTheme } from "@/hooks/use-app-theme";

export function RelatedGameRail({
  games,
  getHref,
}: {
  games: GameBrowseDto[];
  getHref: (gameId: string) => Href;
}) {
  const colors = useAppTheme();
  const router = useRouter();

  return (
    <ScrollView
      horizontal
      showsHorizontalScrollIndicator={false}
      contentContainerStyle={styles.content}
    >
      {games.map((game) => (
        <Pressable
          key={game.id}
          accessibilityRole="button"
          accessibilityLabel={`View ${game.name}`}
          onPress={() => router.push(getHref(game.id))}
          style={({ pressed }) => [
            styles.card,
            { backgroundColor: colors.surfaceHigh, opacity: pressed ? 0.72 : 1 },
          ]}
        >
          {getGameCardImage(game) ? (
            <Image source={getGameCardImage(game)} style={styles.image} contentFit="cover" />
          ) : (
            <View
              style={[
                styles.image,
                styles.placeholder,
                { backgroundColor: colors.primaryContainer },
              ]}
            >
              <Text style={[styles.initial, { color: colors.onPrimaryContainer }]}>
                {game.name.slice(0, 1)}
              </Text>
            </View>
          )}
          <Text numberOfLines={2} style={[styles.name, { color: colors.text }]}>
            {game.name}
          </Text>
        </Pressable>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  content: {
    gap: 10,
    paddingRight: 32,
  },
  card: {
    borderRadius: 16,
    borderCurve: "continuous",
    gap: 8,
    overflow: "hidden",
    paddingBottom: 10,
    width: 132,
  },
  image: {
    aspectRatio: 0.72,
    width: "100%",
  },
  placeholder: {
    alignItems: "center",
    justifyContent: "center",
  },
  initial: {
    fontSize: 30,
    fontWeight: "900",
  },
  name: {
    fontSize: 13,
    fontWeight: "800",
    lineHeight: 17,
    paddingHorizontal: 10,
  },
});
