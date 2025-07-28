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
                //queryClient.cancelQueries({ queryKey: ["games", newHype] });
                queryClient.cancelQueries({ queryKey: ["games"] });
                const previousGames = queryClient.getQueryData<{
                    items: GameDto[];
                }>(["games"]);

                queryClient.setQueryData<{ items: GameDto[] }>(
                    ["games", "similar"],
                    (oldData) => {
                        if (!oldData) return oldData;

                        return {
                            ...oldData,
                            items: oldData.items.map((game) =>
                                game.id === newHype.gameId
                                    ? { ...game, isHyped: true }
                                    : game
                            ),
                        };
                    }
                );
                return { previousGames, newHype };
            },
            onError: (err, newHype, context) => {
                queryClient.setQueryData(["games"], context?.newHype.gameId);
            },
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
                //queryClient.cancelQueries({ queryKey: ["games", newHype] });
                queryClient.cancelQueries({ queryKey: ["games"] });
                const previousGames = queryClient.getQueryData<{
                    items: GameDto[];
                }>(["games"]);

                queryClient.setQueryData<{ items: GameDto[] }>(
                    ["games", "similar"],
                    (oldData) => {
                        if (!oldData) return oldData;

                        return {
                            ...oldData,
                            items: oldData.items.map((game) =>
                                game.id === newHype.gameId
                                    ? { ...game, isHyped: false }
                                    : game
                            ),
                        };
                    }
                );
                return { previousGames, newHype };
            },
            onError: (err, newHype, context) => {
                queryClient.setQueryData(["games"], context?.newHype.gameId);
            },
            onSettled: () => {
                queryClient.invalidateQueries({
                    queryKey: ["games"],
                });
            },
        },
    });

    const toggleHype = (game: GameDto, isCurrentlyHyped: boolean) => {
        if (!isAuthenticated) {
            notice({
                title: "Authentication error",
                description: "You need to be signed in to hype games!",
                status: "error",
                placement: "bottom-right",
            });
            return;
        }

        if (isCurrentlyHyped) {
            mutateRemoveHype.mutate(
                { gameId: game.id },
                {
                    onSuccess: () => {
                        notice({
                            title: `${game.name}`,
                            description: "Game hype removed successfully!",
                            status: "info",
                        });
                        //queryClient.invalidateQueries({ queryKey: ["games"] });
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
                { gameId: game.id },
                {
                    onSuccess: () => {
                        notice({
                            title: `${game.name}`,
                            description: "Game hype hyped successfully!",
                            status: "success",
                        });
                        //queryClient.invalidateQueries({ queryKey: ["games"] });
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
