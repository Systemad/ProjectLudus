"use client";

import { useState } from "react";
import type { GameBrowseDto } from "@src/gen/catalogApi";
import { AspectRatio } from "@astryxdesign/core/AspectRatio";
import { Text } from "@astryxdesign/core/Text";
import { ClickableCard } from "@astryxdesign/core/ClickableCard";
import { Overlay } from "@astryxdesign/core/Overlay";
import { MediaTheme } from "@astryxdesign/core/theme";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import * as stylex from "@stylexjs/stylex";

type Props = {
    game: GameBrowseDto;
};

const styles = stylex.create({
    card: {
        overflow: "hidden",
    },
    media: {
        position: "relative",
        overflow: "hidden",
    },
    image: {
        display: "block",
        width: "100%",
        height: "100%",
        objectFit: "cover",
        transformOrigin: "center",
        transitionDuration: "var(--duration-medium)",
        transitionProperty: "transform",
        "@media (prefers-reduced-motion: reduce)": {
            transitionDuration: "0ms",
        },
    },
    imageActive: {
        transform: "scale(1.04)",
    },
    title: {
        fontWeight: 600,
        lineHeight: 1.25,
    },
});

export function GameCard({ game }: Props) {
    const name = game.name ?? "Unknown";
    const gameId = String(game.id);
    const [isMediaActive, setIsMediaActive] = useState(false);

    return (
        <ClickableCard
            label={name}
            href={`/games/${gameId}?tab=players`}
            variant="transparent"
            padding={0}
            xstyle={styles.card}
            onMouseEnter={() => setIsMediaActive(true)}
            onMouseLeave={() => setIsMediaActive(false)}
            onFocus={() => setIsMediaActive(true)}
            onBlur={() => setIsMediaActive(false)}
        >
            <Overlay
                position="bottom"
                align="start"
                content={
                    <MediaTheme mode="dark">
                        <Text xstyle={styles.title}>{name}</Text>
                    </MediaTheme>
                }
            >
                <div {...stylex.props(styles.media)}>
                    <AspectRatio ratio={3 / 4}>
                        <img
                            src={getIGDBImageUrl(game.coverUrl, "1080p")}
                            alt={name}
                            {...stylex.props(styles.image, isMediaActive && styles.imageActive)}
                        />
                    </AspectRatio>
                </div>
            </Overlay>
        </ClickableCard>
    );
}
