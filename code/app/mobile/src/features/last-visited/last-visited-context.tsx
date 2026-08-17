import { useGamesGetHeroes } from "@/gen/hooks/GamesHooks/useGamesGetHeroes";
import type { GameHeroDto } from "@/gen/types/GameHeroDto";
import { createContext, useCallback, useContext, useEffect, useRef, type ReactNode } from "react";

import { gameIdSchema, type GameId } from "./last-visited-storage";
import { useLastVisitedStore } from "./last-visited-store";

type LastVisitedContextValue = {
  games: { id: GameId; game: GameHeroDto }[];
  isHydrating: boolean;
  isLoading: boolean;
  isError: boolean;
  retry: () => void;
  remember: (gameId: string) => void;
};

const LastVisitedContext = createContext<LastVisitedContextValue | null>(null);

export function LastVisitedProvider({ children }: { children: ReactNode }) {
  const gameIds = useLastVisitedStore((state) => state.lastVisitedGameIds);
  const hasHydrated = useLastVisitedStore((state) => state.hasHydrated);
  const rememberGame = useLastVisitedStore((state) => state.rememberGame);
  const removeGames = useLastVisitedStore((state) => state.removeGames);
  const pendingGameIds = useRef<GameId[]>([]);

  useEffect(() => {
    const applyPendingGameId = () => {
      if (pendingGameIds.current.length === 0) {
        return;
      }

      const nextGameIds = pendingGameIds.current;
      pendingGameIds.current = [];
      nextGameIds.forEach((nextGameId) => rememberGame(nextGameId));
    };

    const unsubscribe = useLastVisitedStore.persist.onFinishHydration(applyPendingGameId);
    if (useLastVisitedStore.persist.hasHydrated()) {
      applyPendingGameId();
    }

    return unsubscribe;
  }, [rememberGame]);

  const remember = useCallback(
    (nextGameId: string) => {
      const result = gameIdSchema.safeParse(nextGameId);
      if (!result.success) {
        return;
      }

      if (!useLastVisitedStore.persist.hasHydrated()) {
        pendingGameIds.current = [
          ...pendingGameIds.current.filter((gameId) => gameId !== result.data),
          result.data,
        ];
      }

      rememberGame(result.data);
    },
    [rememberGame],
  );

  const gameQuery = useGamesGetHeroes(
    { query: { gameIds } },
    { query: { enabled: hasHydrated && gameIds.length > 0 } },
  );

  const games = gameIds.flatMap((id) => {
    const game = gameQuery.data?.games.find((item) => item.id === id);
    return id && game ? [{ id, game }] : [];
  });

  useEffect(() => {
    if (hasHydrated && gameQuery.isSuccess) {
      const availableGameIds = new Set(gameQuery.data.games.map((game) => game.id));
      const missingGameIds = gameIds.filter((id) => !availableGameIds.has(id));

      if (missingGameIds.length > 0) {
        removeGames(missingGameIds);
      }
    }
  }, [gameIds, gameQuery.data, gameQuery.isSuccess, hasHydrated, removeGames]);

  const isLoading = gameQuery.isLoading;
  const isError = gameIds.length > 0 && gameQuery.isError;

  const { refetch } = gameQuery;
  const retry = useCallback(() => {
    void refetch();
  }, [refetch]);

  return (
    <LastVisitedContext.Provider
      value={{
        games,
        isHydrating: !hasHydrated,
        isLoading,
        isError,
        retry,
        remember,
      }}
    >
      {children}
    </LastVisitedContext.Provider>
  );
}

export function useLastVisited() {
  const context = useContext(LastVisitedContext);
  if (!context) {
    throw new Error("useLastVisited must be used inside LastVisitedProvider");
  }

  return context;
}
