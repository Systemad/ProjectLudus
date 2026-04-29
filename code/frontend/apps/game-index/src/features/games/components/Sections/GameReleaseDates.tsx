"use client";

import { Box, Container, For, Grid, GridItem, HStack, Text, VStack } from "ui";
import type { GameReleaseDto } from "@src/gen/catalogApi/types/GameReleaseDto";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import {
    EU,
    US,
    AU,
    NZ,
    JP,
    CN,
    KR,
    BR,
} from "country-flag-icons/react/3x2";

type Props = {
    releaseDates: GameReleaseDto[];
};

const regionFlagMap: Record<string, typeof EU> = {
    europe: EU,
    north_america: US,
    australia: AU,
    new_zealand: NZ,
    japan: JP,
    china: CN,
    korea: KR,
    brazil: BR,
};

export function GameReleaseDates({ releaseDates }: Props) {
    return (
        <Grid templateColumns={{ base: "1fr", md: "1fr 1fr" }} gap="4">
            <For
                each={releaseDates}
                fallback={<Text color="fg.subtle">Release date data is not available.</Text>}
            >
                {(release) => {
                    const RegionFlag = release.region
                        ? regionFlagMap[release.region.toLowerCase()]
                        : null;

                    return (
                        <GridItem
                            key={`${release.platformSlug ?? "unknown"}-${release.region ?? "any"}-${release.releaseDate ?? ""}`}
                        >
                            <Container.Root rounded="lg" p="md">
                                <VStack align="stretch" gap="3">
                                    <HStack align="start" justify="space-between" gap="4">
                                        <VStack align="start" gap="1">
                                            <Text fontSize="xs" color="fg.muted" textTransform="uppercase" letterSpacing="widest">
                                                Platform
                                            </Text>
                                            <HStack gap="1.5" align="center">
                                                {release.platformSlug && (
                                                    <PlatformIcon type={release.platformSlug} />
                                                )}
                                                <Text fontSize="sm" fontWeight="semibold" color="fg.base">
                                                    {release.platformName ?? "Unknown platform"}
                                                </Text>
                                            </HStack>
                                        </VStack>

                                        <VStack align="end" gap="1">
                                            <Text fontSize="xs" color="fg.muted" textTransform="uppercase" letterSpacing="widest">
                                                Release date
                                            </Text>
                                            <Text fontSize="sm" fontWeight="semibold" color="fg.base">
                                                {release.human ??
                                                    (release.releaseDate
                                                        ? new Date(release.releaseDate * 1000).toLocaleDateString()
                                                        : "Unknown")}
                                            </Text>
                                        </VStack>
                                    </HStack>

                                    {release.region ? (
                                        <Box>
                                            <Text fontSize="xs" color="fg.muted" textTransform="uppercase" letterSpacing="widest">
                                                Region
                                            </Text>
                                            <HStack gap="1.5" align="center" mt="1">
                                                {RegionFlag && <RegionFlag style={{ width: "1.2em", height: "1em" }} />}
                                                <Text fontSize="sm" fontWeight="semibold" color="fg.base">
                                                    {release.region}
                                                </Text>
                                            </HStack>
                                        </Box>
                                    ) : null}

                                    <HStack gap="4" wrap="wrap">
                                        {release.developers.length > 0 && (
                                            <Box>
                                                <Text fontSize="xs" color="fg.muted" textTransform="uppercase" letterSpacing="widest">
                                                    Developer{release.developers.length > 1 ? "s" : ""}
                                                </Text>
                                                <Text fontSize="sm" fontWeight="semibold" color="fg.base">
                                                    {release.developers.map((d) => d.name).join(", ")}
                                                </Text>
                                            </Box>
                                        )}
                                        {release.publishers.length > 0 && (
                                            <Box>
                                                <Text fontSize="xs" color="fg.muted" textTransform="uppercase" letterSpacing="widest">
                                                    Publisher{release.publishers.length > 1 ? "s" : ""}
                                                </Text>
                                                <Text fontSize="sm" fontWeight="semibold" color="fg.base">
                                                    {release.publishers.map((p) => p.name).join(", ")}
                                                </Text>
                                            </Box>
                                        )}
                                    </HStack>
                                </VStack>
                            </Container.Root>
                        </GridItem>
                    );
                }}
            </For>
        </Grid>
    );
}
