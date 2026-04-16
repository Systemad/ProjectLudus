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

    return (
        <Card.Root>
            <Card.Header justifyContent="center">
                <Image
                    src={imageUrl}
                    alt={hit.name ? `${hit.name} cover` : "Game cover"}
                    w="full"
                    objectFit="cover"
                />
            </Card.Header>
            <Card.Body>
                <Heading size="sm" lineClamp={2} minH="2.75rem">
                    {hit.name ?? "Untitled game"}
                </Heading>
                <Flex direction="column" gap="xs">
                    <Text fontSize="sm" color="fg.muted">
                        {getDevelopersLabel(hit.developers)} - {getReleaseYear(hit)}
                    </Text>
                </Flex>
            </Card.Body>
            <Card.Footer>
                <Text fontSize="sm" fontWeight="semibold">
                    {rating !== null
                        ? `Rating: ${rating}/100 (${hit.aggregated_rating_count ?? 0} votes)`
                        : "No rating yet"}
                </Text>
            </Card.Footer>
        </Card.Root>
    );
}
