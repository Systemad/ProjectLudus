import { Image } from "expo-image";
import { type ReactNode, useState } from "react";
import { Pressable, ScrollView, StyleSheet, Text, useWindowDimensions, View } from "react-native";
import Animated, { FadeIn, LinearTransition, ReduceMotion } from "react-native-reanimated";

import { PAGE_GUTTER } from "@/config/layout";
import { useAppTheme } from "@/hooks/use-app-theme";

type GameDetailShellProps = {
  title: string;
  eyebrow: string;
  summary: string;
  imageUrl?: string;
  children: ReactNode;
};

const RATING_PLACEHOLDERS = [
  { label: "IGDB rating", value: "—" },
  { label: "IGDB users", value: "—" },
  { label: "Steam reviews", value: "—" },
];

export function GameDetailShell({
  title,
  eyebrow,
  summary,
  imageUrl,
  children,
}: GameDetailShellProps) {
  const colors = useAppTheme();
  const { width } = useWindowDimensions();
  const [summaryExpanded, setSummaryExpanded] = useState(false);
  const mediaHeight = Math.min(Math.max(width * 0.88, 350), 430);
  const coverWidth = Math.min(width * 0.58, 250);

  return (
    <ScrollView
      style={{ backgroundColor: colors.background }}
      contentContainerStyle={styles.content}
      contentInsetAdjustmentBehavior="never"
    >
      <View style={[styles.media, { height: mediaHeight }]}>
        {imageUrl ? (
          <>
            <Image
              source={imageUrl}
              style={styles.backdrop}
              contentFit="cover"
              blurRadius={34}
              transition={250}
            />
            <View style={[styles.scrim, { backgroundColor: colors.background }]} />
            <Image
              source={imageUrl}
              style={[
                styles.cover,
                {
                  width: coverWidth,
                  height: coverWidth / 0.72,
                  backgroundColor: colors.surfaceHigh,
                },
              ]}
              contentFit="cover"
              transition={250}
            />
          </>
        ) : (
          <View style={[styles.mediaFallback, { backgroundColor: colors.surfaceHigh }]} />
        )}
        <View
          pointerEvents="none"
          style={[
            styles.mediaFade,
            {
              experimental_backgroundImage: [
                {
                  type: "linear-gradient",
                  direction: "to bottom",
                  colorStops: [
                    { color: null, positions: ["0%"] },
                    { color: colors.background, positions: ["100%"] },
                  ],
                },
              ],
            },
          ]}
        />
      </View>

      <View style={styles.record}>
        <View style={styles.heading}>
          <Text style={[styles.eyebrow, { color: colors.primary }]}>{eyebrow.toUpperCase()}</Text>
          <Text selectable style={[styles.title, { color: colors.text }]}>
            {title}
          </Text>
        </View>

        <View style={[styles.ratings, { backgroundColor: colors.surfaceHigh }]}>
          {RATING_PLACEHOLDERS.map((rating, index) => (
            <View
              key={rating.label}
              accessibilityLabel={`${rating.label} unavailable`}
              style={[
                styles.rating,
                index > 0 && {
                  borderLeftColor: colors.outline,
                  borderLeftWidth: StyleSheet.hairlineWidth,
                },
              ]}
            >
              <Text
                selectable
                style={[styles.ratingValue, { color: colors.text }]}
                importantForAccessibility="no"
              >
                {rating.value}
              </Text>
              <Text style={[styles.ratingLabel, { color: colors.textMuted }]}>{rating.label}</Text>
            </View>
          ))}
        </View>

        <Animated.View
          layout={LinearTransition.duration(220).reduceMotion(ReduceMotion.System)}
          style={styles.synopsis}
        >
          <Text
            selectable
            numberOfLines={summaryExpanded ? undefined : 5}
            style={[styles.summary, { color: colors.textMuted }]}
          >
            {summary}
          </Text>
          <Pressable
            accessibilityRole="button"
            accessibilityState={{ expanded: summaryExpanded }}
            onPress={() => setSummaryExpanded((current) => !current)}
            style={({ pressed }) => [styles.summaryToggle, { opacity: pressed ? 0.65 : 1 }]}
          >
            <Animated.Text
              entering={FadeIn.duration(160).reduceMotion(ReduceMotion.System)}
              style={[styles.summaryToggleLabel, { color: colors.primary }]}
            >
              {summaryExpanded ? "Show less" : "Read more"}
            </Animated.Text>
          </Pressable>
        </Animated.View>
      </View>

      {children}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  content: {
    paddingBottom: 110,
    gap: 22,
  },
  media: {
    width: "100%",
    overflow: "hidden",
    alignItems: "center",
    justifyContent: "center",
  },
  backdrop: {
    ...StyleSheet.absoluteFill,
    transform: [{ scale: 1.16 }],
  },
  scrim: {
    ...StyleSheet.absoluteFill,
    opacity: 0.64,
  },
  mediaFade: {
    position: "absolute",
    right: 0,
    bottom: 0,
    left: 0,
    height: 112,
  },
  cover: {
    borderRadius: 24,
    borderCurve: "continuous",
  },
  mediaFallback: {
    width: "58%",
    maxWidth: 250,
    aspectRatio: 0.72,
    borderRadius: 24,
    borderCurve: "continuous",
  },
  record: {
    paddingHorizontal: PAGE_GUTTER,
    gap: 18,
  },
  heading: {
    gap: 6,
  },
  eyebrow: {
    fontSize: 12,
    fontWeight: "900",
    letterSpacing: 1.2,
  },
  title: {
    fontSize: 32,
    lineHeight: 36,
    fontWeight: "900",
  },
  ratings: {
    minHeight: 68,
    flexDirection: "row",
    overflow: "hidden",
    borderRadius: 18,
    borderCurve: "continuous",
  },
  rating: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center",
    paddingHorizontal: 6,
    gap: 3,
  },
  ratingValue: {
    fontSize: 20,
    lineHeight: 24,
    fontWeight: "900",
    fontVariant: ["tabular-nums"],
  },
  ratingLabel: {
    fontSize: 11,
    lineHeight: 14,
    fontWeight: "700",
    textAlign: "center",
  },
  synopsis: {
    gap: 4,
  },
  summary: {
    fontSize: 16,
    lineHeight: 24,
  },
  summaryToggle: {
    minHeight: 36,
    alignSelf: "flex-start",
    justifyContent: "center",
  },
  summaryToggleLabel: {
    fontSize: 14,
    lineHeight: 18,
    fontWeight: "800",
  },
});
