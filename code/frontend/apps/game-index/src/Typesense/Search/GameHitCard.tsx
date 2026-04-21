import { Box, Flex, Heading, Image, Text, Card } from "ui";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { getDevelopersLabel, getReleaseYear } from "../utils/searchUtils";
import type { GameSearchHit } from "../utils/hits";

type GameHitCardProps = {
    hit: GameSearchHit;
};

export function GameHitCard({ hit }: GameHitCardProps) {
    const imageUrl = getIGDBImageUrl(hit.cover_url, "cover_big");
    const rating =
        typeof hit.aggregated_rating === "number" ? Math.round(hit.aggregated_rating) : null;
    const votes = hit.aggregated_rating_count ?? 0;
    const releaseYear = getReleaseYear(hit);

    return (
        <Card.Root
            h="full"
            rounded="lg"
            bg="bg.surface"
            borderWidth="1px"
            borderColor="border.subtle"
            overflow="hidden"
        >
            <Card.Header justifyContent="center">
                <Image
                    src={imageUrl}
                    alt={hit.name ? `${hit.name} cover` : "Game cover"}
                    w="full"
                    h={{ base: "4xs", md: "3xs" }}
                    objectFit="cover"
                />
            </Card.Header>
            <Card.Body p={{ base: "xs", md: "sm" }}>
                <Heading size="sm" lineClamp={2} minH="2.5rem" color="fg.base">
                    {hit.name ?? "Untitled game"}
                </Heading>
                <Flex direction="column" gap="2xs" mt="xs">
                    <Text fontSize="sm" color="fg.subtle" lineClamp={1}>
                        {getDevelopersLabel(hit.developers)}
                    </Text>
                    <Text fontSize="sm" color="fg.muted">
                        Released: {releaseYear}
                    </Text>
                </Flex>
            </Card.Body>
            <Card.Footer p={{ base: "xs", md: "sm" }} pt="0">
                <Box
                    rounded="md"
                    bg="bg.panel"
                    borderWidth="1px"
                    borderColor="border.subtle"
                    px="sm"
                    py="xs"
                    w="full"
                >
                    <Text fontSize="sm" color="fg.subtle" lineHeight="short">
                        Rating:{" "}
                        <Text as="span" color="fg.base" fontWeight="semibold">
                            {rating !== null ? `${rating}/100` : "No rating yet"}
                        </Text>
                        {rating !== null ? ` (${votes} votes)` : ""}
                    </Text>
                </Box>
            </Card.Footer>
        </Card.Root>
    );
}
