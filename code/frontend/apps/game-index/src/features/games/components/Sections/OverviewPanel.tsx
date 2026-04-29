"use client";
import { Box, HStack, Tag, Text, VStack, Wrap, For } from "ui";
import { sectionMetaStyle } from "@src/utils/sectionTextStyles";
import { PlatformIcon } from "@src/icons/PlatformIcon";

type Props = {
    gameTypeName?: string | null;
    modes: string[];
    playerPerspectives: string[];
    platforms: string[];
    genres: string[];
    themes: string[];
};

export function OverviewPanel({
    gameTypeName,
    modes,
    playerPerspectives,
    platforms,
    genres,
    themes,
}: Props) {
    return (
        <VStack align="stretch" gap={6}>
            <Box>
                <Text {...sectionMetaStyle} mb={3}>
                    PLATFORMS
                </Text>
                <VStack align="stretch" gap={2}>
                    <For each={platforms}>
                        {(platform) => (
                            <HStack key={platform} gap="xs">
                                <PlatformIcon type={platform} />
                                <Text colorScheme="neutral" fontSize="sm">
                                    {platform}
                                </Text>
                            </HStack>
                        )}
                    </For>
                </VStack>
            </Box>

            <Box>
                <Text {...sectionMetaStyle} mb={3}>
                    GAME MODES
                </Text>
                <Wrap gap="xs">
                    <For each={modes}>
                        {(mode) => (
                            <Tag
                                key={mode}
                                variant="subtle"
                                colorScheme="gray"
                                size="sm"
                                textTransform="none"
                            >
                                {mode}
                            </Tag>
                        )}
                    </For>
                </Wrap>
            </Box>

            <Box>
                <Text {...sectionMetaStyle} mb={3}>
                    GENRES & THEMES
                </Text>
                <Wrap gap="xs">
                    <For each={[...genres, ...themes]}>
                        {(item) => (
                            <Tag
                                key={item}
                                variant="surface"
                                colorScheme="gray"
                                size="sm"
                                textTransform="none"
                            >
                                {item}
                            </Tag>
                        )}
                    </For>
                </Wrap>
            </Box>

            {playerPerspectives.length > 0 && (
                <Box>
                    <Text {...sectionMetaStyle} mb={3}>
                        PERSPECTIVES
                    </Text>
                    <Wrap gap="xs">
                        <For each={playerPerspectives}>
                            {(perspective) => (
                                <Tag
                                    key={perspective}
                                    variant="subtle"
                                    colorScheme="gray"
                                    size="sm"
                                    textTransform="none"
                                >
                                    {perspective}
                                </Tag>
                            )}
                        </For>
                    </Wrap>
                </Box>
            )}

            {gameTypeName && (
                <Box>
                    <Text {...sectionMetaStyle} mb={3}>
                        TYPE
                    </Text>
                    <Tag variant="subtle" colorScheme="gray" size="sm" textTransform="none">
                        {gameTypeName}
                    </Tag>
                </Box>
            )}
        </VStack>
    );
}
