"use client";

import { Text } from "@astryxdesign/core/Text";
import { Timestamp } from "@astryxdesign/core/Timestamp";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { Grid } from "@astryxdesign/core/Grid";
import { Divider } from "@astryxdesign/core/Divider";
import { DataCard } from "@src/components/data-card";
import type { GameReleaseDto } from "@src/gen/catalogApi/types/GameReleaseDto";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import { EU, US, AU, NZ, JP, CN, KR, BR } from "country-flag-icons/react/3x2";

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
        <Grid columns={{minWidth: 280}} gap={4}>
            {releaseDates.length > 0 ? (
                releaseDates.map((release) => {
                    const RegionFlag = release.region
                        ? regionFlagMap[release.region.toLowerCase()]
                        : null;
                    const developers = release.involvedCompanies
                        .filter((company) => company.developer)
                        .map((company) => company.companyName);
                    const publishers = release.involvedCompanies
                        .filter((company) => company.publisher)
                        .map((company) => company.companyName);

                    return (
                        <DataCard
                            key={`${release.platformSlug ?? "unknown"}-${release.region ?? "any"}-${release.releaseDate ?? ""}`}
                            padding={4}
                        >
                            <VStack hAlign="stretch" gap={3}>
                                <HStack vAlign="start" hAlign="between" gap={3}>
                                    <VStack hAlign="start" gap={1}>
                                        <Text type="label" color="secondary">Platform</Text>
                                        <HStack gap={2} vAlign="center">
                                            {release.platformSlug && (
                                                <PlatformIcon type={release.platformSlug} tooltip={release.platformName ?? release.platformSlug} />
                                            )}
                                            <Text weight="semibold">{release.platformName ?? "Unknown platform"}</Text>
                                        </HStack>
                                    </VStack>
                                    <VStack hAlign="end" gap={1}>
                                        <Text type="label" color="secondary">Release date</Text>
                                        {release.human ? (
                                            <Text weight="semibold">{release.human}</Text>
                                        ) : release.releaseDate ? (
                                            <Timestamp value={release.releaseDate} format="date" weight="semibold" />
                                        ) : (
                                            <Text weight="semibold">Unknown</Text>
                                        )}
                                    </VStack>
                                </HStack>
                                <Divider />
                                <Grid columns={{minWidth: 120}} gap={3}>
                                    {release.region && (
                                        <VStack hAlign="start" gap={1}>
                                            <Text type="label" color="secondary">Region</Text>
                                            <HStack gap={2} vAlign="center">
                                                {RegionFlag && <RegionFlag width="1.2em" height="1em" />}
                                                <Text weight="semibold">{release.region}</Text>
                                            </HStack>
                                        </VStack>
                                    )}
                                    {developers.length > 0 && (
                                        <VStack hAlign="start" gap={1}>
                                            <Text type="label" color="secondary">Developer{developers.length > 1 ? "s" : ""}</Text>
                                            <Text weight="semibold">{developers.join(", ")}</Text>
                                        </VStack>
                                    )}
                                    {publishers.length > 0 && (
                                        <VStack hAlign="start" gap={1}>
                                            <Text type="label" color="secondary">Publisher{publishers.length > 1 ? "s" : ""}</Text>
                                            <Text weight="semibold">{publishers.join(", ")}</Text>
                                        </VStack>
                                    )}
                                </Grid>
                            </VStack>
                        </DataCard>
                    );
                })
            ) : (
                <Text style={{color: "var(--fg-tertiary)"}}>Release date data is not available.</Text>
            )}
        </Grid>
    );
}
