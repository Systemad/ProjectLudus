import { Image } from "expo-image";
import { router } from "expo-router";
import { Settings } from "lucide-react-native";
import type { ReactNode } from "react";
import { Pressable, Text, View } from "react-native";

import { useAppTheme } from "@/hooks/use-app-theme";
import { settingsHref } from "@/navigation/routes";

export function SignedOutProfile({
  children,
  colors,
  loading = false,
  isSigningIn = false,
  onSignIn,
}: {
  children?: ReactNode;
  colors: ReturnType<typeof useAppTheme>;
  loading?: boolean;
  isSigningIn?: boolean;
  onSignIn?: () => void;
}) {
  return (
    <View style={{ backgroundColor: colors.background, flex: 1, padding: 16 }}>
      <View style={{ alignItems: "flex-end", height: 48, justifyContent: "center" }}>
        <Pressable
          accessibilityLabel="Settings"
          accessibilityRole="button"
          hitSlop={8}
          onPress={() => router.push(settingsHref)}
        >
          <Settings color={colors.text} size={24} />
        </Pressable>
      </View>
      <View style={{ alignItems: "center", flex: 1, justifyContent: "center" }}>
        {children ??
          (loading ? (
            <Text style={{ color: colors.text, fontSize: 18 }}>Loading profile…</Text>
          ) : (
            <>
              <Image
                source={require("@/assets/images/icon.png")}
                style={{ height: 88, width: 88 }}
                contentFit="contain"
              />
              <Text style={{ color: colors.text, fontSize: 28, fontWeight: "800", marginTop: 16 }}>
                GameIndex
              </Text>
              <Text
                style={{
                  color: colors.text,
                  fontSize: 17,
                  lineHeight: 24,
                  marginTop: 12,
                  textAlign: "center",
                }}
              >
                Sign in with Steam to link your inventory and stats.
              </Text>
              <Pressable
                accessibilityRole="button"
                disabled={isSigningIn}
                onPress={() => onSignIn?.()}
                style={{
                  alignItems: "center",
                  backgroundColor: colors.primary,
                  borderRadius: 24,
                  justifyContent: "center",
                  marginTop: 24,
                  minHeight: 52,
                  paddingHorizontal: 28,
                  width: "100%",
                }}
              >
                <Text style={{ color: colors.onPrimary, fontSize: 16, fontWeight: "700" }}>
                  {isSigningIn ? "Opening Steam…" : "Sign in with Steam"}
                </Text>
              </Pressable>
              <View
                style={{
                  borderColor: colors.outline,
                  borderRadius: 16,
                  borderWidth: 1,
                  marginTop: 24,
                  padding: 16,
                  width: "100%",
                }}
              >
                <Text
                  style={{
                    color: colors.text,
                    fontSize: 14,
                    fontWeight: "700",
                    textAlign: "center",
                  }}
                >
                  Uses official Steam OpenID
                </Text>
                <Text
                  style={{
                    color: colors.text,
                    fontSize: 14,
                    marginTop: 4,
                    textAlign: "center",
                  }}
                >
                  We never see your password.
                </Text>
              </View>
            </>
          ))}
      </View>
    </View>
  );
}
