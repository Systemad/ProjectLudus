import { useNotice } from "@yamada-ui/react";
import {
    useMeHypesAddEndpointHook,
    useMeHypesRemoveEndpointHook,
    type GameDto,
} from "~/gen";
import { useAuth } from "~/features/auth/useAuth";
import { queryClient } from "~/queryClient";
export function useHype() {
    const { isAuthenticated } = useAuth();
    const notice = useNotice({ limit: 3, placement: "bottom-right" });
    const mutateAddHype = useMeHypesAddEndpointHook({
        mutation: {
            onMutate: async (newHype) => {
                queryClient.cancelQueries({ queryKey: ["games"] });
                const previousGames = queryClient.getQueryData<{
                    items: GameDto[];
                }>(["games"]);

                queryClient.setQueryData<{ items: GameDto[] }>(
                    ["games"],
                    (oldData) => {
                        if (!oldData) return oldData;

                        return {
                            ...oldData,
                            items: oldData.items.map((game) =>
                                game.id === newHype.gameId
                                    ? { ...game, isHyped: !game.isHyped }
                                    : game
                            ),
                        };
                    }
                );
                return { previousGames, newHype };
            },
            /*
            onError: (err, newHype, context) => {
                queryClient.setQueryData(["games"], context?.newHype.gameId);
            },
            */
            onSettled: () => {
                queryClient.invalidateQueries({
                    queryKey: ["games"],
                });
            },
        },
    });
    const mutateRemoveHype = useMeHypesRemoveEndpointHook({
        mutation: {
            onMutate: async (newHype) => {
                queryClient.cancelQueries({ queryKey: ["games"] });
                const previousGames = queryClient.getQueryData<{
                    items: GameDto[];
                }>(["games"]);

                queryClient.setQueryData<{ items: GameDto[] }>(
                    ["games"],
                    (oldData) => {
                        if (!oldData) return oldData;

                        return {
                            ...oldData,
                            items: oldData.items.map((game) =>
                                game.id === newHype.gameId
                                    ? { ...game, isHyped: !game.isHyped }
                                    : game
                            ),
                        };
                    }
                );
                return { previousGames, newHype };
            },

            onSettled: () => {
                queryClient.invalidateQueries({
                    queryKey: ["games"],
                });
            },
        },
    });

    const toggleHype = (gameId: number, isHyped: boolean, gameName: string) => {
        if (!isAuthenticated) {
            notice({
                title: "Authentication error",
                description: "You need to be signed in to hype games!",
                status: "error",
                placement: "bottom-right",
            });
            return;
        }

        if (isHyped) {
            mutateRemoveHype.mutate(
                { gameId: gameId },
                {
                    onSuccess: () => {
                        notice({
                            title: `${gameName}`,
                            description: "Game hype removed successfully!",
                            status: "info",
                        });
                    },
                    onError: () => {
                        notice({
                            title: "Error",
                            description: "Failed to remove hype.",
                            status: "error",
                        });
                    },
                }
            );
        } else {
            mutateAddHype.mutate(
                { gameId: gameId },
                {
                    onSuccess: () => {
                        notice({
                            title: `${gameName}`,
                            description: "Game hype hyped successfully!",
                            status: "success",
                        });
                    },
                    onError: () => {
                        notice({
                            title: "Error",
                            description: "Failed to add hype.",
                            status: "error",
                        });
                    },
                }
            );
        }
    };

    return {
        toggleHype,
        isLoading: mutateAddHype.isPending || mutateRemoveHype.isPaused,
    };
}
