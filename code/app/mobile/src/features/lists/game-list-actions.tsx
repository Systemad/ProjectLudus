import { BottomSheet, Button, Checkbox, Column, Host, ListItem, Text } from "@expo/ui";
import { Activity, Heart, ListPlus, Share2 } from "lucide-react-native";
import { useState } from "react";
import { Pressable, StyleSheet, Text as NativeText, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";

import { useGameDetailActions } from "./use-game-detail-actions";

export function GameListActions({ gameId }: { gameId: string }) {
  const colors = useAppTheme();
  const actions = useGameDetailActions(gameId);
  const [sheetVisible, setSheetVisible] = useState(false);
  const [trackingPlayers, setTrackingPlayers] = useState(false);

  const openListSheet = () => {
    if (actions.signedIn) setSheetVisible(true);
    else actions.goToProfile();
  };

  return (
    <>
      <View style={styles.row}>
        <ActionSurface
          colors={colors}
          icon={
            <Heart
              color={colors.primary}
              fill={actions.membership.data?.isWishlisted ? colors.primary : "transparent"}
              size={21}
            />
          }
          label={actions.membership.data?.isWishlisted ? "Wishlisted" : "Wishlist"}
          selected={actions.membership.data?.isWishlisted ?? false}
          onPress={() => {
            if (!actions.pending) void actions.toggleWishlist();
          }}
        />
        <ActionSurface
          colors={colors}
          icon={<ListPlus color={colors.primary} size={21} />}
          label="Save to list"
          onPress={openListSheet}
        />
        <ActionSurface
          colors={colors}
          icon={<Activity color={colors.primary} size={21} />}
          label={trackingPlayers ? "Tracking" : "Track players"}
          selected={trackingPlayers}
          onPress={() => setTrackingPlayers((value) => !value)}
        />
        <ActionSurface
          colors={colors}
          icon={<Share2 color={colors.primary} size={21} />}
          label="Share"
          onPress={actions.share}
        />
      </View>
      {sheetVisible ? (
        <Host pointerEvents="box-none" style={StyleSheet.absoluteFill}>
          <BottomSheet
            isPresented={sheetVisible}
            onDismiss={() => setSheetVisible(false)}
            snapPoints={["half", "full"]}
          >
            <Column spacing={12}>
              <Text textStyle={{ fontSize: 21, fontWeight: "800" }}>Save to a list</Text>
              {actions.lists.data?.map((list) => {
                const selected = actions.membership.data?.listIds.includes(list.id) ?? false;
                return (
                  <ListItem
                    key={list.id}
                    onPress={() => void actions.toggleList(list.id)}
                    supportingText={`${list.itemCount} games`}
                    trailing={
                      <Checkbox
                        value={selected}
                        disabled={actions.pending}
                        onValueChange={() => void actions.toggleList(list.id)}
                      />
                    }
                  >
                    {list.name}
                  </ListItem>
                );
              })}
              <Button label="Create new list" variant="outlined" onPress={actions.createList} />
            </Column>
          </BottomSheet>
        </Host>
      ) : null}
    </>
  );
}

function ActionSurface({
  colors,
  icon,
  label,
  selected = false,
  onPress,
}: {
  colors: ReturnType<typeof useAppTheme>;
  icon: React.ReactNode;
  label: string;
  selected?: boolean;
  onPress: () => void;
}) {
  return (
    <Pressable
      accessibilityRole="button"
      accessibilityLabel={label}
      onPress={onPress}
      style={({ pressed }) => [
        styles.action,
        {
          backgroundColor: selected ? colors.primaryContainer : colors.surfaceHigh,
          borderColor: colors.outline,
          opacity: pressed ? 0.7 : 1,
        },
      ]}
    >
      {icon}
      <NativeText
        style={{ color: colors.text, fontSize: 11, fontWeight: "800", textAlign: "center" }}
        numberOfLines={2}
      >
        {label}
      </NativeText>
    </Pressable>
  );
}

const styles = StyleSheet.create({
  row: {
    flexDirection: "row",
    gap: 8,
  },
  action: {
    alignItems: "center",
    borderRadius: 16,
    borderWidth: StyleSheet.hairlineWidth,
    borderCurve: "continuous",
    flex: 1,
    gap: 7,
    justifyContent: "center",
    minHeight: 82,
    paddingHorizontal: 4,
    paddingVertical: 10,
  },
});
