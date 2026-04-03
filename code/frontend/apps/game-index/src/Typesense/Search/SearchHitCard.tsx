import { Box, Flex, Heading, Image, Text } from "ui";
import {
    getCoverImageUrl,
    getDevelopersLabel,
    getReleaseYear,
    type GameSearchHit,
} from "./searchUtils";

type SearchHitCardProps = {
    hit: GameSearchHit;
};

export function SearchHitCard({ hit }: SearchHitCardProps) {
    const imageUrl = getCoverImageUrl(hit.cover_url);
    const rating =
        typeof hit.aggregated_rating === "number" ? Math.round(hit.aggregated_rating) : null;

    return (
        <Box
            as="article"
            h="full"
            display="grid"
            gridTemplateRows="auto 1fr"
            gap="sm"
            p="sm"
            rounded="xl"
            layerStyle="translucentPanel"
        >
            <Box aspectRatio="3/4" overflow="hidden" rounded="lg" bg="blackAlpha.400">
                {imageUrl ? (
                    <Image
                        src={imageUrl}
                        alt={hit.name ? `${hit.name} cover` : "Game cover"}
                        w="full"
                        h="full"
                        objectFit="cover"
                    />
                ) : (
                    <Flex w="full" h="full" align="center" justify="center">
                        <Text fontSize="sm" color="fg.muted">
                            No cover image
                        </Text>
                    </Flex>
                )}
            </Box>

            <Flex direction="column" gap="xs">
                <Heading size="sm" lineClamp={2} minH="2.75rem">
                    {hit.name ?? "Untitled game"}
                </Heading>

                <Text fontSize="sm" color="fg.muted">
                    {getDevelopersLabel(hit.developers)} - {getReleaseYear(hit)}
                </Text>

                <Text fontSize="sm" fontWeight="semibold">
                    {rating !== null
                        ? `Rating: ${rating}/100 (${hit.aggregated_rating_count ?? 0} votes)`
                        : "No rating yet"}
                </Text>
            </Flex>
        </Box>
    );
}
