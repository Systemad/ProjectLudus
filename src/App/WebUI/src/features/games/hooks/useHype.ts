import { useNotice } from "@yamada-ui/react";
import {
    useMeHypesAddEndpoint,
    useMeHypesRemoveEndpoint,
    type GameDto,
} from "~/api";
import { useAuth } from "~/features/auth/useAuth";
import { queryClient } from "~/queryClient";
export function useHype() {
    const { isAuthenticated } = useAuth();
    const notice = useNotice({ limit: 3, placement: "bottom-right" });
    const mutateAddHype = useMeHypesAddEndpoint();
    const mutateRemoveHype = useMeHypesRemoveEndpoint();

    const toggleHype = (game: GameDto, isCurrentlyHyped: boolean) => {
        if (!isAuthenticated) {
            notice({
                title: "Notification",
                description: "This is description.",
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

                        // TODO: look over this, maybe invalidate all queries related to "api/games"
                        queryClient.invalidateQueries({
                            queryKey: [{ url: "/api/games/top" }],
                            exact: false,
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
                { gameId: game.id },
                {
                    onSuccess: () => {
                        notice({
                            title: `${game.name}`,
                            description: "Game hype hyped successfully!",
                            status: "success",
                        });
                        queryClient.invalidateQueries({
                            queryKey: [{ url: "/api/games/top" }],
                            exact: false,
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
