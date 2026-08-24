import { create } from "zustand";
import { persist } from "zustand/middleware";

import {
  MAX_LAST_VISITED,
  lastVisitedStorage,
  type GameId,
  type PersistedLastVisitedState,
} from "./last-visited-storage";

type LastVisitedStore = PersistedLastVisitedState & {
  hasHydrated: boolean;
  rememberGame: (gameId: GameId) => void;
  removeGames: (gameIds: GameId[]) => void;
  setHasHydrated: (hasHydrated: boolean) => void;
};

export const useLastVisitedStore = create<LastVisitedStore>()(
  persist(
    (set) => ({
      lastVisitedGameIds: [],
      hasHydrated: false,
      rememberGame: (gameId) => {
        set((state) => ({
          lastVisitedGameIds: [
            gameId,
            ...state.lastVisitedGameIds.filter((visitedGameId) => visitedGameId !== gameId),
          ].slice(0, MAX_LAST_VISITED),
        }));
      },
      removeGames: (gameIds) => {
        const removedGameIds = new Set(gameIds);

        set((state) => {
          const nextGameIds = state.lastVisitedGameIds.filter(
            (visitedGameId) => !removedGameIds.has(visitedGameId),
          );

          return nextGameIds.length === state.lastVisitedGameIds.length
            ? state
            : { lastVisitedGameIds: nextGameIds };
        });
      },
      setHasHydrated: (hasHydrated) => set({ hasHydrated }),
    }),
    {
      name: "game-index.last-visited",
      storage: lastVisitedStorage,
      partialize: ({ lastVisitedGameIds }) => ({ lastVisitedGameIds }),
      version: 2,
      onRehydrateStorage: () => (state) => {
        if (state) {
          state.setHasHydrated(true);
        } else {
          useLastVisitedStore.setState({ hasHydrated: true });
        }
      },
    },
  ),
);
