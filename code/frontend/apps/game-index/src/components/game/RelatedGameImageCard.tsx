"use client";

import { AspectRatio, Box, Image, Text } from "ui";

type Props = {
    name: string;
    imageUrl?: string;
};

export default function GameCard({ name, imageUrl }: Props) {
    return (
        <Box role="group">
            <Box position="relative" rounded="xl" overflow="hidden">
                <AspectRatio ratio={3 / 4}>
                    <Image
                        src={imageUrl || undefined}
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
    );
}
