"use client";

import { Text } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { Grid } from "@astryxdesign/core/Grid";
import { Section } from "@astryxdesign/core/Section";
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

                    return (
                        <div
                            key={`${release.platformSlug ?? "unknown"}-${release.region ?? "any"}-${release.releaseDate ?? ""}`}
                        >
                            <Section
                                variant="muted"
                                padding={4}
                                style={{ borderRadius: "var(--radius-lg)" }}
                            >
                                <VStack hAlign="stretch" gap={3}>
                                    <HStack vAlign="start" hAlign="between" gap={4}>
                                        <VStack hAlign="start" gap={1}>
                                            <Text
                                                color="secondary"
                                                style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                                            >
                                                Platform
                                            </Text>
                                            <HStack gap={2} vAlign="center">
                                                {release.platformSlug && (
                                                    <PlatformIcon type={release.platformSlug} tooltip={release.platformName ?? release.platformSlug} />
                                                )}
                                                <Text weight="semibold" style={{fontSize: "0.875rem"}}>
                                                    {release.platformName ?? "Unknown platform"}
                                                </Text>
                                            </HStack>
                                        </VStack>

                                        <VStack hAlign="end" gap={1}>
                                            <Text
                                                color="secondary"
                                                style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                                            >
                                                Release date
                                            </Text>
                                            <Text weight="semibold" style={{fontSize: "0.875rem"}}>
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
                                        <div>
                                            <Text
                                                color="secondary"
                                                style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                                            >
                                                Region
                                            </Text>
                                            <HStack gap={2} vAlign="center" style={{marginTop: "0.25rem"}}>
                                                {RegionFlag && (
                                                    <RegionFlag width="1.2em" height="1em" />
                                                )}
                                                <Text weight="semibold" style={{fontSize: "0.875rem"}}>
                                                    {release.region}
                                                </Text>
                                            </HStack>
                                        </div>
                                    ) : null}

                                    <HStack gap={4} wrap="wrap">
                                        {release.involvedCompanies.filter((c) => c.developer)
                                            .length > 0 && (
                                            <div>
                                                <Text
                                                    color="secondary"
                                                    style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                                                >
                                                    Developer
                                                    {release.involvedCompanies.filter(
                                                        (c) => c.developer,
                                                    ).length > 1
                                                        ? "s"
                                                        : ""}
                                                </Text>
                                                <Text weight="semibold" style={{fontSize: "0.875rem"}}>
                                                    {release.involvedCompanies
                                                        .filter((c) => c.developer)
                                                        .map((c) => c.companyName)
                                                        .join(", ")}
                                                </Text>
                                            </div>
                                        )}
                                        {release.involvedCompanies.filter((c) => c.publisher)
                                            .length > 0 && (
                                            <div>
                                                <Text
                                                    color="secondary"
                                                    style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                                                >
                                                    Publisher
                                                    {release.involvedCompanies.filter(
                                                        (c) => c.publisher,
                                                    ).length > 1
                                                        ? "s"
                                                        : ""}
                                                </Text>
                                                <Text weight="semibold" style={{fontSize: "0.875rem"}}>
                                                    {release.involvedCompanies
                                                        .filter((c) => c.publisher)
                                                        .map((c) => c.companyName)
                                                        .join(", ")}
                                                </Text>
                                            </div>
                                        )}
                                    </HStack>
                                </VStack>
                            </Section>
                        </div>
                    );
                })
            ) : (
                <Text style={{color: "var(--fg-tertiary)"}}>Release date data is not available.</Text>
            )}
        </Grid>
    );
}
