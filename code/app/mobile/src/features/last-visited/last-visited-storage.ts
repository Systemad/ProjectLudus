import AsyncStorage from "@react-native-async-storage/async-storage";
import { z } from "zod";
import { createJSONStorage, type PersistStorage } from "zustand/middleware";

export const gameIdSchema = z.string().regex(/^\d+$/);
export type GameId = z.infer<typeof gameIdSchema>;

const persistedLastVisitedStateSchema = z.object({
  lastVisitedGameId: gameIdSchema.nullable(),
});

export type PersistedLastVisitedState = z.infer<typeof persistedLastVisitedStateSchema>;

const persistedStorageValueSchema = z.object({
  state: persistedLastVisitedStateSchema,
  version: z.number().optional(),
});

const jsonStorage = createJSONStorage(() => AsyncStorage);

if (!jsonStorage) {
  throw new Error("AsyncStorage is required for last visited state.");
}

const validatedJsonStorage = jsonStorage;

async function discardStoredState(name: string) {
  try {
    await validatedJsonStorage.removeItem(name);
  } catch {
    return;
  }
}

export const lastVisitedStorage: PersistStorage<PersistedLastVisitedState> = {
  getItem: async (name) => {
    try {
      const storedState = await validatedJsonStorage.getItem(name);
      if (storedState === null) {
        return null;
      }

      const result = persistedStorageValueSchema.safeParse(storedState);
      if (result.success) {
        return result.data;
      }
    } catch {
      await discardStoredState(name);
      return null;
    }

    await discardStoredState(name);
    return null;
  },
  setItem: async (name, value) => {
    if (value.state.lastVisitedGameId === null) {
      await discardStoredState(name);
      return null;
    }

    await validatedJsonStorage.setItem(name, value);
  },
  removeItem: async (name) => {
    await discardStoredState(name);
  },
};
