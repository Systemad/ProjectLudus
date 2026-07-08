"use client";

import { Link } from "@tanstack/react-router";
import type { GameBrowseDto } from "@src/gen/catalogApi";
import { AspectRatio } from "@astryxdesign/core/AspectRatio";
import { Text } from "@astryxdesign/core/Text";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Props = {
    game: GameBrowseDto;
};

export function GameCard({ game }: Props) {
    const name = game.name ?? "Unknown";
    const gameId = String(game.id);

    return (
        <Link
            to="/games/$gameId"
            params={{ gameId }}
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <div role="group">
                <div style={{ position: "relative", borderRadius: "var(--radius-xl)", overflow: "hidden" }}>
                    <AspectRatio ratio={3 / 4}>
                        <img
                            src={getIGDBImageUrl(game.coverUrl, "1080p")}
                            alt={name}
                            style={{
                                width: "100%",
                                height: "100%",
                                objectFit: "cover",
                                transitionDuration: "0.5s",
                                transitionProperty: "transform",
                            }}
                        />
                    </AspectRatio>
                    <Text
                        style={{
                            position: "absolute",
                            left: "0.75rem",
                            right: "0.75rem",
                            bottom: "0.75rem",
                            color: "white",
                            fontWeight: 600,
                            lineHeight: "1.25",
                        }}
                    >
                        {name}
                    </Text>
                </div>
            </div>
        </Link>
    );
}
