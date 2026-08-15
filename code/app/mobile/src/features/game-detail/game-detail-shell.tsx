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
  const mediaHeight = Math.min(Math.max(width * 0.62, 250), 320);
  const coverWidth = Math.min(width * 0.34, 132);

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
    alignItems: "flex-start",
    justifyContent: "flex-end",
    paddingHorizontal: PAGE_GUTTER,
    paddingBottom: 16,
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
    borderRadius: 16,
    borderCurve: "continuous",
  },
  mediaFallback: {
    width: "34%",
    maxWidth: 132,
    aspectRatio: 0.72,
    borderRadius: 16,
    borderCurve: "continuous",
  },
  record: {
    paddingHorizontal: PAGE_GUTTER,
    gap: 14,
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
    fontSize: 28,
    lineHeight: 32,
    fontWeight: "900",
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
