"use client";

import { Button, GlobeIcon, Gamepad2Icon, Grid, HStack, Icon, Text } from "ui";
import { CardSurface } from "../layout/Card";
import { platformIconMap } from "@src/icons/platformIconMap";
import type { ExternalGameDto, WebsiteDto } from "@src/gen/catalogApi";

import { sectionLabelStyle, sectionMetaStyle } from "@src/utils/sectionTextStyles";
type Props = {
    websites: WebsiteDto[];
    externalGames: ExternalGameDto[];
};

function OfficialLinks({ websites, externalGames }: Props) {
    return (
        <CardSurface variant="translucent" p={6}>
            <Text {...sectionLabelStyle} mb={3}>
                Official Links
            </Text>
            {websites.length > 0 ? (
                <Grid templateColumns={{ base: "1fr", md: "1fr 1fr" }} gap={3}>
                    {websites.map((website) => (
                        <Button
                            key={website.id}
                            as="a"
                            href={website.url ?? undefined}
                            target="_blank"
                            rel="noreferrer"
                            variant="ghost"
                            colorScheme="gray"
                            color="fg.base"
                            justifyContent="space-between"
                            endIcon={<span>›</span>}
                            size="sm"
                            _hover={{
                                bg: "bg.subtle",
                                color: "fg.base",
                            }}
                        >
                            <HStack gap="2xs">
                                {platformIconMap[website.typeName?.toLowerCase() ?? ""] ? (
                                    platformIconMap[website.typeName?.toLowerCase() ?? ""]
                                ) : (
                                    <Icon
                                        as={
                                            website.typeName?.toLowerCase().includes("official")
                                                ? GlobeIcon
                                                : Gamepad2Icon
                                        }
                                        color="fg.muted"
                                    />
                                )}
                                <Text>{website.typeName ?? "Official Link"}</Text>
                            </HStack>
                        </Button>
                    ))}
                </Grid>
            ) : (
                <Text color="fg.subtle" fontSize="sm">
                    No official links available.
                </Text>
            )}
            {externalGames.length > 0 && (
                <>
                    <Text {...sectionMetaStyle} mt={6} mb={3}>
                        AVAILABLE ON
                    </Text>
                    <Grid templateColumns={{ base: "1fr", md: "1fr 1fr" }} gap={3}>
                        {externalGames.map((eg) => (
                            <Button
                                key={eg.id}
                                as="a"
                                href={eg.url ?? undefined}
                                target="_blank"
                                rel="noreferrer"
                                variant="ghost"
                                colorScheme="gray"
                                color="fg.base"
                                justifyContent="space-between"
                                endIcon={<span>›</span>}
                                size="sm"
                                _hover={{
                                    bg: "bg.subtle",
                                    color: "fg.base",
                                }}
                            >
                                <HStack gap="2xs">
                                    {platformIconMap[
                                        eg.externalGameSourceName?.toLowerCase() ?? ""
                                    ] ?? <Icon as={Gamepad2Icon} color="fg.muted" />}
                                    <Text>{eg.externalGameSourceName ?? eg.name ?? "Store"}</Text>
                                </HStack>
                            </Button>
                        ))}
                    </Grid>
                </>
            )}
        </CardSurface>
    );
}

export default OfficialLinks;
