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
            onMutate: async (newWishlist) => {
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
                                game.id === newWishlist.gameId
                                    ? {
                                          ...game,
                                          isWishlisted: !game.isWishlisted,
                                      }
                                    : game
                            ),
                        };
                    }
                );
                return { previousGames, newWishlist };
            },
            /*
            onError: (err, newHype, context) => {
                queryClient.setQueryData(["games"], context?.newHype.gameId);
            },
            */
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
            onMutate: async (newWishlist) => {
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
                                game.id === newWishlist.gameId
                                    ? {
                                          ...game,
                                          isWishlisted: !game.isWishlisted,
                                      }
                                    : game
                            ),
                        };
                    }
                );
                return { previousGames, newWishlist };
            },
            onSettled: () => {
                queryClient.invalidateQueries({
                    queryKey: ["games"],
                });
            },
        },
    });

    const toggleWishlist = (
        gameId: number,
        isWishlisted: boolean,
        gameName: string
    ) => {
        if (!isAuthenticated) {
            notice({
                title: "Authentication error",
                description: "You need to be signed in to hype games!",
                status: "error",
                placement: "bottom-right",
            });
            return;
        }

        if (isWishlisted) {
            mutateRemoveHype.mutate(
                { gameId: gameId },
                {
                    onSuccess: () => {
                        notice({
                            title: `${gameName}`,
                            description: "Game wishlist removed successfully!",
                            status: "info",
                        });
                        //queryClient.invalidateQueries({ queryKey: ["games"] });
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
                { gameId: gameId },
                {
                    onSuccess: () => {
                        notice({
                            title: `${gameName}`,
                            description: "Game wishlisted successfully!",
                            status: "success",
                        });
                        //queryClient.invalidateQueries({ queryKey: ["games"] });
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
