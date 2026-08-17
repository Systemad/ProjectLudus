import AsyncStorage from "@react-native-async-storage/async-storage";
import { z } from "zod";
import { createJSONStorage, type PersistStorage } from "zustand/middleware";

export const gameIdSchema = z.string().regex(/^\d+$/);
export type GameId = z.infer<typeof gameIdSchema>;

export const MAX_LAST_VISITED = 10;

const persistedLastVisitedStateSchema = z.object({
  lastVisitedGameIds: z.array(gameIdSchema).max(MAX_LAST_VISITED),
});

export type PersistedLastVisitedState = z.infer<typeof persistedLastVisitedStateSchema>;

const legacyPersistedLastVisitedStateSchema = z.object({
  lastVisitedGameId: gameIdSchema.nullable(),
});

const persistedStorageValueSchema = z.object({
  state: z.union([persistedLastVisitedStateSchema, legacyPersistedLastVisitedStateSchema]),
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
        if ("lastVisitedGameIds" in result.data.state) {
          return { state: result.data.state, version: 2 };
        }

        return {
          state: {
            lastVisitedGameIds:
              result.data.state.lastVisitedGameId === null
                ? []
                : [result.data.state.lastVisitedGameId],
          },
          version: 2,
        };
      }
    } catch {
      await discardStoredState(name);
      return null;
    }

    await discardStoredState(name);
    return null;
  },
  setItem: async (name, value) => {
    if (value.state.lastVisitedGameIds.length === 0) {
      await discardStoredState(name);
      return null;
    }

    await validatedJsonStorage.setItem(name, value);
  },
  removeItem: async (name) => {
    await discardStoredState(name);
  },
};
