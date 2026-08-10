import * as SecureStore from "expo-secure-store";

import { authTokenSchema } from "./auth-schemas";

const KEY = "game-index.token";

export const authStorage = {
  get: async () => {
    const rawToken = await SecureStore.getItemAsync(KEY);
    const result = authTokenSchema.safeParse(rawToken);

    if (!result.success) {
      if (rawToken !== null) {
        await SecureStore.deleteItemAsync(KEY);
      }

      return null;
    }

    return result.data;
  },
  set: async (token: string) => {
    await SecureStore.setItemAsync(KEY, authTokenSchema.parse(token));
  },
  clear: () => SecureStore.deleteItemAsync(KEY),
};
