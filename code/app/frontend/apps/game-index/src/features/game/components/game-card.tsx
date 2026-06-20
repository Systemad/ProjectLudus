"use client";

import { Link } from "@tanstack/react-router";
import type { GameBrowseDto } from "@src/gen/catalogApi";

import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { Box, Text } from "ui";

type GameCardModel = Pick<GameBrowseDto, "name"> & {
    id: number | string;
    coverUrl?: string | null;
};

type Props = {
    game: GameCardModel;
};

export function GameCard({ game }: Props) {
    const gameId = String(game.id);

    return (
        <Link
            to="/games/$gameId"
            params={{ gameId }}
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <Box role="group" width="100%" minWidth="9rem" flexShrink={0}>
                <Box position="relative" borderRadius={800} overflow="hidden">
                    <Box style={{ aspectRatio: "3/4" }}>
                        <Box
                            as="img"
                            src={getIGDBImageUrl(game.coverUrl, "1080p")}
                            alt={game.name ? `${game.name} cover` : "Game cover"}
                            width="100%"
                            height="100%"
                            style={{ objectFit: "cover", transition: "transform 0.5s" }}
                            _groupHover={{ transform: "scale(1.08)" }}
                        />
                    </Box>
                    <Text
                        position="absolute"
                        left={3}
                        right={3}
                        bottom={3}
                        color="fgInverse"
                        font="body1"
                    >
                        {game.name}
                    </Text>
                </Box>
            </Box>
        </Link>
    );
}
