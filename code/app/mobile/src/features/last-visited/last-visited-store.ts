import { create } from "zustand";
import { persist } from "zustand/middleware";

import {
  lastVisitedStorage,
  type GameId,
  type PersistedLastVisitedState,
} from "./last-visited-storage";

type LastVisitedStore = PersistedLastVisitedState & {
  hasHydrated: boolean;
  setLastVisitedGameId: (gameId: GameId) => void;
  clearLastVisitedGame: () => void;
  setHasHydrated: (hasHydrated: boolean) => void;
};

export const useLastVisitedStore = create<LastVisitedStore>()(
  persist(
    (set) => ({
      lastVisitedGameId: null,
      hasHydrated: false,
      setLastVisitedGameId: (gameId) => {
        set({ lastVisitedGameId: gameId });
      },
      clearLastVisitedGame: () => set({ lastVisitedGameId: null }),
      setHasHydrated: (hasHydrated) => set({ hasHydrated }),
    }),
    {
      name: "game-index.last-visited",
      storage: lastVisitedStorage,
      partialize: ({ lastVisitedGameId }) => ({ lastVisitedGameId }),
      version: 1,
      onRehydrateStorage: () => (state) => {
        state?.setHasHydrated(true);
      },
    },
  ),
);
