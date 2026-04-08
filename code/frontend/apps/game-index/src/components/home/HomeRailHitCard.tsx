import { Link } from "@tanstack/react-router";
import { Box, Flex, Image, Text, VStack } from "ui";
import { getCoverImageUrl, type GameSearchHit } from "@src/Typesense/Search/searchUtils";

export type HomeRailHit = GameSearchHit & { objectID?: string };

export const homeRailHitClassNames = {
    list: "typesense-release-list",
    item: "typesense-release-item",
} as const;

export const homeRailHitListCss = {
    ".typesense-release-list": {
        display: "grid",
        gridTemplateColumns: "repeat(auto-fill, minmax(220px, 1fr))",
        gap: "0.75rem",
        listStyle: "none",
        margin: 0,
        padding: 0,
    },
    ".typesense-release-item": {
        margin: 0,
    },
} as const;

function formatReleaseDate(firstReleaseDate?: number | string) {
    let date: Date | null = null;

    if (typeof firstReleaseDate === "number") {
        date = new Date(firstReleaseDate * 1000);
    } else if (typeof firstReleaseDate === "string") {
        const parsed = new Date(firstReleaseDate);
        if (!Number.isNaN(parsed.getTime())) date = parsed;
    }

    if (!date) return "Release date TBA";

    return new Intl.DateTimeFormat("en-US", {
        month: "short",
        day: "numeric",
        year: "numeric",
        timeZone: "UTC",
    }).format(date);
}

export function HomeRailHitCard({ hit }: { hit: HomeRailHit }) {
    return (
        <Box
            as="article"
            rounded="2xl"
            bg="bg.panel"
            borderWidth="1px"
            borderColor="border.subtle"
            p="4"
        >
            <Flex gap="3" align="center">
                <Image
                    src={getCoverImageUrl(hit.cover_url)}
                    alt={hit.name ?? "Game cover"}
                    w="12"
                    h="12"
                    minW="12"
                    rounded="xl"
                    objectFit="cover"
                    flexShrink={0}
                    bg="bg.subtle"
                />
                <VStack align="stretch" gap="0.5" minW={0}>
                    <Link
                        to="/games/$gameId"
                        params={{ gameId: String(hit.id) }}
                        style={{ color: "inherit", textDecoration: "none" }}
                    >
                        <Text
                            fontWeight="bold"
                            fontSize="sm"
                            whiteSpace="nowrap"
                            overflow="hidden"
                            textOverflow="ellipsis"
                        >
                            {hit.name ?? "Untitled game"}
                        </Text>
                    </Link>
                    <Text color="fg.muted" fontSize="xs">
                        {formatReleaseDate(hit.first_release_date)}
                    </Text>
                </VStack>
            </Flex>
        </Box>
    );
}
