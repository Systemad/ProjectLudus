import { Image } from "expo-image";
import { type Href, useRouter } from "expo-router";
import { ChevronRight } from "lucide-react-native";
import { Pressable, StyleSheet, Text, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";
import { commonStyles } from "@/shared/ui/common-styles";

type CatalogRowProps = {
  href: Href;
  title: string;
  subtitle: string;
  imageUrl?: string;
};

export function CatalogRow({ href, title, subtitle, imageUrl }: CatalogRowProps) {
  const colors = useAppTheme();
  const router = useRouter();

  return (
    <Pressable
      accessibilityRole="button"
      accessibilityLabel={`View ${title}`}
      onPress={() => router.push(href)}
      style={({ pressed }) => [
        commonStyles.surfaceCard,
        commonStyles.row,
        styles.row,
        {
          backgroundColor: colors.surfaceHigh,
          opacity: pressed ? 0.8 : 1,
        },
      ]}
    >
      {imageUrl ? (
        <Image source={imageUrl} style={styles.image} contentFit="cover" />
      ) : (
        <View style={[styles.placeholder, { backgroundColor: colors.primaryContainer }]}>
          <Text style={[styles.initial, { color: colors.onPrimaryContainer }]}>
            {title.slice(0, 1)}
          </Text>
        </View>
      )}
      <View style={styles.copy}>
        <Text style={[styles.title, { color: colors.text }]} numberOfLines={1}>
          {title}
        </Text>
        <Text style={[styles.subtitle, { color: colors.textMuted }]} numberOfLines={1}>
          {subtitle}
        </Text>
      </View>
      <ChevronRight color={colors.textMuted} size={22} strokeWidth={2.2} />
    </Pressable>
  );
}

const styles = StyleSheet.create({
  row: {
    width: "100%",
    minHeight: 68,
    paddingHorizontal: 12,
    paddingVertical: 10,
  },
  image: {
    width: 46,
    height: 46,
    borderRadius: 12,
    borderCurve: "continuous",
  },
  placeholder: {
    width: 46,
    height: 46,
    borderRadius: 12,
    borderCurve: "continuous",
    alignItems: "center",
    justifyContent: "center",
  },
  initial: {
    fontSize: 20,
    fontWeight: "800",
  },
  copy: {
    flex: 1,
    minWidth: 0,
    gap: 2,
  },
  title: {
    fontSize: 16,
    lineHeight: 20,
    fontWeight: "700",
  },
  subtitle: {
    fontSize: 13,
    lineHeight: 18,
  },
});
