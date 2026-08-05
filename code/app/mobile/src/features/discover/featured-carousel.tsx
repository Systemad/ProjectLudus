import { StyleSheet, useWindowDimensions } from "react-native";
import Animated, {
  Extrapolation,
  interpolate,
  useAnimatedScrollHandler,
  useAnimatedStyle,
  useSharedValue,
  type SharedValue,
} from "react-native-reanimated";

import { PAGE_GUTTER } from "@/config/layout";
import { GameCard } from "@/entities/game/game-card";
import type { GameBrowseDto } from "@/gen/types/GameBrowseDto";
import type { Href } from "expo-router";

const CARD_GAP = 10;
const VISIBLE_CARD_COUNT = 3;
const ACTIVE_SCALE = 1.08;
const INACTIVE_SCALE = 0.88;

type FeaturedCarouselProps = {
  games: GameBrowseDto[];
  getHref: (game: GameBrowseDto) => Href;
};

export function FeaturedCarousel({ games, getHref }: FeaturedCarouselProps) {
  const { width } = useWindowDimensions();
  const scrollX = useSharedValue(0);
  const availableWidth = width - PAGE_GUTTER * 2;
  const cardWidth = Math.floor(
    (availableWidth - CARD_GAP * (VISIBLE_CARD_COUNT - 1)) / VISIBLE_CARD_COUNT,
  );
  const cardHeight = cardWidth / 0.72;

  const itemInterval = cardWidth + CARD_GAP;
  const onScroll = useAnimatedScrollHandler((event) => {
    scrollX.value = event.contentOffset.x;
  });

  return (
    <Animated.FlatList
      horizontal
      data={games}
      keyExtractor={(game) => String(game.id)}
      decelerationRate={0.92}
      onScroll={onScroll}
      removeClippedSubviews={false}
      renderItem={({ item, index }) => {
        return (
          <FeaturedCarouselCard
            game={item}
            href={getHref(item)}
            index={index}
            cardHeight={cardHeight}
            cardWidth={cardWidth}
            itemInterval={itemInterval}
            scrollX={scrollX}
          />
        );
      }}
      showsHorizontalScrollIndicator={false}
      snapToAlignment="start"
      snapToInterval={itemInterval}
      scrollEventThrottle={16}
      contentContainerStyle={styles.content}
    />
  );
}

type FeaturedCarouselCardProps = {
  game: GameBrowseDto;
  href: Href;
  index: number;
  cardWidth: number;
  cardHeight: number;
  itemInterval: number;
  scrollX: SharedValue<number>;
};

function FeaturedCarouselCard({
  game,
  href,
  index,
  cardWidth,
  cardHeight,
  itemInterval,
  scrollX,
}: FeaturedCarouselCardProps) {
  const animatedStyle = useAnimatedStyle(() => {
    const centerOffset = (index - 1) * itemInterval;
    const distanceFromCenter = Math.abs(scrollX.value - centerOffset);

    return {
      transform: [
        {
          scale: interpolate(
            distanceFromCenter,
            [0, itemInterval],
            [ACTIVE_SCALE, INACTIVE_SCALE],
            Extrapolation.CLAMP,
          ),
        },
      ],
      zIndex: interpolate(distanceFromCenter, [0, itemInterval], [2, 0], Extrapolation.CLAMP),
    };
  });

  return (
    <Animated.View
      style={[
        styles.cardFrame,
        { width: cardWidth, height: cardHeight * ACTIVE_SCALE + 4 },
        animatedStyle,
      ]}
    >
      <GameCard game={game} variant="cover" href={href} />
    </Animated.View>
  );
}

const styles = StyleSheet.create({
  content: {
    alignItems: "center",
    gap: CARD_GAP,
    paddingVertical: 2,
  },
  cardFrame: {
    justifyContent: "center",
  },
});
