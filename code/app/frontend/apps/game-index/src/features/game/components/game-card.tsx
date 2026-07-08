"use client";

import { Link } from "@tanstack/react-router";
import type { GameBrowseDto } from "@src/gen/catalogApi";

import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { Text } from "@astryxdesign/core/Text";

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
            <div role="group" style={{ width: "100%", minWidth: "9rem", flexShrink: 0 }}>
                <div style={{ position: "relative", borderRadius: 800, overflow: "hidden" }}>
                    <div style={{ aspectRatio: "3/4" }}>
                        <img
                            src={getIGDBImageUrl(game.coverUrl, "1080p")}
                            alt={game.name ? `${game.name} cover` : "Game cover"}
                            style={{
                                width: "100%",
                                height: "100%",
                                objectFit: "cover",
                                transition: "transform 0.5s",
                            }}
                        />
                    </div>
                    <Text
                        style={{
                            position: "absolute",
                            left: "0.75rem",
                            right: "0.75rem",
                            bottom: "0.75rem",
                            color: "white",
                        }}
                    >
                        {game.name}
                    </Text>
                </div>
            </div>
        </Link>
    );
}
