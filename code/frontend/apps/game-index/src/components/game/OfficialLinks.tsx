"use client";

import { Button, GlobeIcon, Gamepad2Icon, Grid, HStack, Icon, Text } from "ui";
import { CardSurface } from "../layout/Card";
import { platformIconMap } from "@src/icons/platformIconMap";
import type { WebsiteDto } from "@src/gen/catalogApi";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";
type Props = {
    websites: WebsiteDto[];
};

export function OfficialLinks({ websites }: Props) {
    return (
        <CardSurface variant="translucent" p={6}>
            <Text {...sectionLabelStyle} mb={3}>
                Official Links
            </Text>
            {websites.length > 0 ? (
                <Grid templateColumns={{ base: "1fr", md: "1fr 1fr" }} gap={3}>
                    {websites.map((website) => (
                        <Button
                            key={website.name}
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
                                {platformIconMap[website.type?.toLowerCase() ?? ""] ? (
                                    platformIconMap[website.type?.toLowerCase() ?? ""]
                                ) : (
                                    <Icon
                                        as={
                                            website.type?.toLowerCase().includes("official")
                                                ? GlobeIcon
                                                : Gamepad2Icon
                                        }
                                        color="fg.muted"
                                    />
                                )}
                                <Text>{website.type ?? "Official Link"}</Text>
                            </HStack>
                        </Button>
                    ))}
                </Grid>
            ) : (
                <Text color="fg.subtle" fontSize="sm">
                    No official links available.
                </Text>
            )}
        </CardSurface>
    );
}
