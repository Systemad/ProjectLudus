"use client";

import { Heading, Text, VStack } from "ui";

export function IGDBInfo() {
    return (
        <VStack align="stretch" gap="4">
            <Heading fontSize="xl" fontWeight="bold" color="fg.base">
                IGDB
            </Heading>
            <Text color="fg.muted">IGDB-specific game data coming soon.</Text>
        </VStack>
    );
}
