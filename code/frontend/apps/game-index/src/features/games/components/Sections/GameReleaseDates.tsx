"use client";

import { Box, For, HStack, Tag, Text, VStack, Wrap } from "ui";
import type { GameReleaseDto } from "@src/gen/catalogApi/types/GameReleaseDto";

type Props = {
    releaseDates: GameReleaseDto[];
};

export function GameReleaseDates({ releaseDates }: Props) {
    return (
        <VStack align="stretch" gap={8}>
            <For
                each={releaseDates}
                fallback={<Text color="fg.subtle">Release date data is not available.</Text>}
            >
                {(release) => (
                    <Box
                        key={`${release.platformSlug ?? "unknown"}-${release.region ?? "any"}-${release.releaseDate ?? ""}`}
                        rounded="lg"
                        p="md"
                    >
                        <HStack align="start" justify="space-between" wrap="wrap" gap="4">
                            <VStack align="start" gap={1}>
                                <Text fontSize="sm" color="fg.muted">
                                    Platform
                                </Text>
                                <Text fontWeight="semibold" color="fg.base">
                                    {release.platformName ?? "Unknown platform"}
                                </Text>
                            </VStack>

                            <VStack align="start" gap={1}>
                                <Text fontSize="sm" color="fg.muted">
                                    Release date
                                </Text>
                                <Text fontWeight="semibold" color="fg.base">
                                    {release.human ??
                                        (release.releaseDate
                                            ? new Date(
                                                  release.releaseDate * 1000,
                                              ).toLocaleDateString()
                                            : "Unknown")}
                                </Text>
                            </VStack>
                        </HStack>

                        {release.region ? (
                            <Text color="fg.subtle" fontSize="sm" mt="3">
                                Region: {release.region}
                            </Text>
                        ) : null}

                        <Wrap gap="2" mt="4">
                            <For each={release.developers}>
                                {(developer) => (
                                    <Tag
                                        key={`dev-${developer.name}`}
                                        variant="surface"
                                        colorScheme="gray"
                                        size="md"
                                    >
                                        Developer: {developer.name}
                                    </Tag>
                                )}
                            </For>
                            <For each={release.publishers}>
                                {(publisher) => (
                                    <Tag
                                        key={`pub-${publisher.name}`}
                                        variant="surface"
                                        colorScheme="gray"
                                        size="sm"
                                    >
                                        Publisher: {publisher.name}
                                    </Tag>
                                )}
                            </For>
                        </Wrap>
                    </Box>
                )}
            </For>
        </VStack>
    );
}
