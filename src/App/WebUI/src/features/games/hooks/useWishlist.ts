import { useNotice } from "@yamada-ui/react";
import {
    useMeWishlistsAddEndpointHook,
    useMeWishlistsRemoveEndpointHook,
    type GameDto,
} from "~/gen";
import { useAuth } from "~/features/auth/useAuth";
import { queryClient } from "~/queryClient";

export function useWishlist() {
    const { isAuthenticated } = useAuth();
    const notice = useNotice({ limit: 3, placement: "bottom-right" });
    const mutateAddHype = useMeWishlistsAddEndpointHook({
        mutation: {
            onMutate: async (newHype) => {
                //queryClient.cancelQueries({ queryKey: ["games", newHype] });
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
                                    ? { ...game, isWishlisted: true }
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
                    //queryKey: ["games", newHype?.id],
                    queryKey: ["games"],
                });
            },
        },
    });
    const mutateRemoveHype = useMeWishlistsRemoveEndpointHook({
        mutation: {
            onMutate: async (newHype) => {
                //queryClient.cancelQueries({ queryKey: ["games", newHype] });
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
                                    ? { ...game, isWishlisted: false }
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
            onSettled: (newHype) => {
                queryClient.invalidateQueries({
                    queryKey: ["games", newHype?.id],
                });
            },
        },
    });

    const toggleWishlist = (game: GameDto, isCurrentlyWishlisted: boolean) => {
        if (!isAuthenticated) {
            notice({
                title: "Authentication error",
                description: "You need to be signed in to hype games!",
                status: "error",
                placement: "bottom-right",
            });
            return;
        }

        console.log(
            queryClient
                .getQueryCache()
                .getAll()
                .map((q) => q.queryKey)
        );

        if (isCurrentlyWishlisted) {
            mutateRemoveHype.mutate(
                { gameId: game.id },
                {
                    onSuccess: () => {
                        notice({
                            title: `${game.name}`,
                            description: "Game wishlist removed successfully!",
                            status: "info",
                        });
                        queryClient.invalidateQueries({ queryKey: ["games"] });
                    },
                    onError: () => {
                        notice({
                            title: "Error",
                            description: "Failed to remove wishlist.",
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
                            description: "Game wishlisted successfully!",
                            status: "success",
                        });
                        queryClient.invalidateQueries({ queryKey: ["games"] });
                    },
                    onError: () => {
                        notice({
                            title: "Error",
                            description: "Failed to add wishlist.",
                            status: "error",
                        });
                    },
                }
            );
        }
    };

    return {
        toggleWishlist,
        isLoading: mutateAddHype.isPending || mutateRemoveHype.isPaused,
    };
}
