"use client";

import { Button, Grid, HStack, Text, Box } from "ui";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import type { WebsiteDto } from "@src/gen/catalogApi";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";
type Props = {
    websites: WebsiteDto[];
};

export function OfficialLinks({ websites }: Props) {
    return (
        <Box>
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
                            colorScheme="link"
                            justifyContent="space-between"
                            endIcon={<span>›</span>}
                            size="sm"
                            _hover={{
                                bg: "bg.subtle",
                                color: "colorScheme.fg",
                            }}
                        >
                            <HStack gap="2xs">
                                <PlatformIcon type={website.type} />
                                <Text color="inherit">{website.type ?? "Official Link"}</Text>
                            </HStack>
                        </Button>
                    ))}
                </Grid>
            ) : (
                <Text color="fg.subtle" fontSize="sm">
                    No official links available.
                </Text>
            )}
        </Box>
    );
}
