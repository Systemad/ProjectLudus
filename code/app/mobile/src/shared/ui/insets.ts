import { useSafeAreaInsets } from "react-native-safe-area-context";

/**
 * SafeAreaProvider is mounted once at the app root. Screens only add their own
 * content spacing on top of the shared bottom gesture inset.
 */
export function useContentBottomInset(extra = 0) {
  return useSafeAreaInsets().bottom + extra;
}
