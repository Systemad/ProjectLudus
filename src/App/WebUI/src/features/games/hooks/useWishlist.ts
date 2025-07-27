import { useNotice } from "@yamada-ui/react";
import {
    useMeWishlistsAddEndpoint,
    useMeWishlistsRemoveEndpoint,
    type GameDto,
} from "~/api";
import { useAuth } from "~/features/auth/useAuth";

export function useWishlist() {
    const { isAuthenticated } = useAuth();
    const notice = useNotice({ limit: 3, placement: "bottom-right" });
    const mutateAddHype = useMeWishlistsAddEndpoint();
    const mutateRemoveHype = useMeWishlistsRemoveEndpoint();

    const toggleWishlist = (game: GameDto, isCurrentlyWishlisted: boolean) => {
        if (!isAuthenticated) {
            notice({
                title: "Notification",
                description: "This is description.",
                status: "error",
                placement: "bottom-right",
            });
            return;
        }

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
