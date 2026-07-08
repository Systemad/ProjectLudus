"use client";

import { Text, Heading } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";

export function IGDBInfo() {
    return (
        <VStack hAlign="stretch" gap={4}>
            <Heading level={3}>
                IGDB
            </Heading>
            <Text color="secondary">IGDB-specific game data coming soon.</Text>
        </VStack>
    );
}
