"use client";

import { Button, Grid, Text, Box, ExternalLinkIcon } from "ui";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import type { WebsiteDto } from "@src/gen/catalogApi";
type Props = {
    websites: WebsiteDto[];
};

export function OfficialLinks({ websites }: Props) {
    return (
        <Box>
            <Text mb={3}>Official Links</Text>
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
                            colorScheme="neutral"
                            justifyContent="flex-start"
                            size="sm"
                            _hover={{
                                bg: "bg.subtle",
                                color: "colorScheme.fg",
                            }}
                        >
                            <ExternalLinkIcon />
                            <PlatformIcon type={website.type!} tooltip={website.type!} />
                            <Text color="inherit">{website.type ?? "Official Link"}</Text>
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
