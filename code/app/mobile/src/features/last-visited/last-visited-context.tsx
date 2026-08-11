import { useGamesGetHero } from "@/gen/hooks/GamesHooks/index";
import type { GameHeroDto } from "@/gen/types/GameHeroDto";
import { createContext, useCallback, useContext, useEffect, useRef, type ReactNode } from "react";

import { gameIdSchema, type GameId } from "./last-visited-storage";
import { useLastVisitedStore } from "./last-visited-store";

type LastVisitedContextValue = {
  gameId: GameId | null;
  game: GameHeroDto | undefined;
  isHydrating: boolean;
  isLoading: boolean;
  isError: boolean;
  retry: () => void;
  remember: (gameId: string) => void;
};

const LastVisitedContext = createContext<LastVisitedContextValue | null>(null);

export function LastVisitedProvider({ children }: { children: ReactNode }) {
  const gameId = useLastVisitedStore((state) => state.lastVisitedGameId);
  const hasHydrated = useLastVisitedStore((state) => state.hasHydrated);
  const setLastVisitedGameId = useLastVisitedStore((state) => state.setLastVisitedGameId);
  const clearLastVisitedGame = useLastVisitedStore((state) => state.clearLastVisitedGame);
  const pendingGameId = useRef<GameId | null>(null);

  useEffect(() => {
    const applyPendingGameId = () => {
      if (pendingGameId.current === null) {
        return;
      }

      const nextGameId = pendingGameId.current;
      pendingGameId.current = null;
      setLastVisitedGameId(nextGameId);
    };

    const unsubscribe = useLastVisitedStore.persist.onFinishHydration(applyPendingGameId);
    if (useLastVisitedStore.persist.hasHydrated()) {
      applyPendingGameId();
    }

    return unsubscribe;
  }, [setLastVisitedGameId]);

  const remember = useCallback(
    (nextGameId: string) => {
      const result = gameIdSchema.safeParse(nextGameId);
      if (!result.success) {
        return;
      }

      if (!useLastVisitedStore.persist.hasHydrated()) {
        pendingGameId.current = result.data;
      }

      setLastVisitedGameId(result.data);
    },
    [setLastVisitedGameId],
  );

  const gameQuery = useGamesGetHero(
    { path: { gameId: gameId ?? "0" } },
    { query: { enabled: hasHydrated && gameId !== null } },
  );

  const isMissing =
    hasHydrated &&
    gameId !== null &&
    (gameQuery.error?.status === 404 || (gameQuery.isSuccess && !gameQuery.data?.game));

  useEffect(() => {
    if (isMissing) {
      clearLastVisitedGame();
    }
  }, [clearLastVisitedGame, isMissing]);

  return (
    <LastVisitedContext.Provider
      value={{
        gameId,
        game: gameQuery.data?.game,
        isHydrating: !hasHydrated,
        isLoading: gameQuery.isLoading,
        isError: gameQuery.isError,
        retry: () => void gameQuery.refetch(),
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
