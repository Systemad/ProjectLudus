"use client";

import { Link } from "@tanstack/react-router";
import type { GameDto } from "@src/gen/catalogApi";
import { AspectRatio, Box, Image, Text } from "ui";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Props = {
    game: GameDto;
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
            <Box role="group">
                <Box position="relative" rounded="xl" overflow="hidden">
                    <AspectRatio ratio={3 / 4}>
                        <Image
                            src={getIGDBImageUrl(game.coverUrl, "1080p")}
                            alt={name}
                            w="full"
                            h="full"
                            objectFit="cover"
                            transitionDuration="slower"
                            transitionProperty="transform"
                            _groupHover={{ transform: "scale(1.08)" }}
                        />
                    </AspectRatio>
                    <Box
                        position="absolute"
                        inset={0}
                        bgGradient="linear(to-t, blackAlpha.900 4%, transparent 60%)"
                        pointerEvents="none"
                    />
                    <Text
                        position="absolute"
                        left={3}
                        right={3}
                        bottom={3}
                        color="white"
                        fontWeight="semibold"
                        lineHeight="short"
                    >
                        {name}
                    </Text>
                </Box>
            </Box>
        </Link>
    );
}
