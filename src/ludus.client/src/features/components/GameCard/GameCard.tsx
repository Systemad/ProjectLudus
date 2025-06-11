import { HeartIcon } from "@phosphor-icons/react";
import { Card, Text, Group, ActionIcon } from "@mantine/core";
import { Link } from "@tanstack/react-router";
import classes from "./GameCard.module.css";
import { useMeFavoritesUpdateEndpoint, type GameDto } from "~/api";
import { IGDBImage } from "../IGDBImage/IGDBImage";
import { queryClient } from "~/main";

type Props = {
    Game: GameDto;
    height?: number;
    fontSize?: "xs" | "sm" | "md" | "lg" | "xl";
    iconSize?: number;
};
export function GameCard({
    Game,
    height = 200,
    fontSize = "lg",
    iconSize = 48,
}: Props) {
    const { mutate, isPending, isError, isSuccess } =
        useMeFavoritesUpdateEndpoint({
            mutation: {
                onSuccess: (data) => {
                    queryClient.invalidateQueries({
                        queryKey: ["games"],
                    });
                },
            },
        });

    const favoriteItem = () => {
        console.log("favorting item");
        mutate({ gameId: Game.id, data: { isFavorited: !Game.isFavorited } });
    };
    return (
        <Card radius={"lg"} withBorder className={classes.item}>
            <Card.Section>
                <Link
                    to={"/games/$gameId"}
                    params={{ gameId: Game.id.toString() }}
                >
                    <IGDBImage
                        fit="cover"
                        size="1080p"
                        imageId={Game.coverImageId}
                        h={height}
                    />
                </Link>
            </Card.Section>

            <Group justify="space-between" mt="md" wrap="nowrap">
                <Text fw={500} size={fontSize}>
                    {Game.name}
                </Text>

                <ActionIcon variant="transparent" aria-label="Favorite">
                    <HeartIcon
                        className={`heart ${isSuccess ? classes.pulse : ""}`}
                        weight={Game.isFavorited ? "fill" : "regular"}
                        color="#904b40"
                        size={iconSize}
                        onClick={() => favoriteItem()}
                    />
                </ActionIcon>
            </Group>
        </Card>
    );
}
