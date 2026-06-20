import { Box, Card, Heading, HStack, Image, Text, VStack } from "ui";
import { useExtractColors } from "react-extract-colors";

import type { GameBrowseDto } from "@src/gen/catalogApi";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { hslToColorScheme } from "@src/utils/colorUtils";
import { steamReviewColor, steamReviewRating } from "@src/utils/SteamReviewUtils";

type Props = {
    game: GameBrowseDto;
};

export function GamePreviewCard({ game }: Props) {
    const coverSrc = game.steam?.headerUrl ?? getIGDBImageUrl(game.coverUrl, "1080p");
    const { dominantColor } = useExtractColors(coverSrc, {
        format: "hsl",
        maxColors: 5,
        sortBy: "vibrance",
        crossOrigin: "anonymous",
    });
    const colorScheme = hslToColorScheme(dominantColor);
    const players = game.steam?.currentPlayers?.toLocaleString();
    const releaseDate = game.firstReleaseDate
        ? new Date(game.firstReleaseDate).toLocaleDateString("en-US", {
              month: "short",
              day: "numeric",
              year: "numeric",
          })
        : "TBA";
    const ratingColor = steamReviewColor(game.review);
    const ratingText = steamReviewRating(game.review);

    return (
        <Card.Root w="64" colorScheme={colorScheme} variant="subtle" overflow="hidden" rounded="xl">
            <Card.Header p="sm">
                <Image
                    src={coverSrc}
                    alt={game.name}
                    w="full"
                    rounded="xl"
                    aspectRatio={16 / 9}
                    objectFit="cover"
                />
            </Card.Header>

            <Card.Body p="sm">
                <Heading size="md" lineClamp={1}>
                    {game.name}
                </Heading>

                <VStack align="stretch" gap="1" fontSize="sm">
                    {players && (
                        <HStack gap="xs">
                            <Text>👥</Text>
                            <Text>{players} current players</Text>
                        </HStack>
                    )}
                    {releaseDate && (
                        <HStack gap="xs">
                            <Text>📅</Text>
                            <Text>{releaseDate}</Text>
                        </HStack>
                    )}
                    {game.pricing?.finalFormatted && (
                        <HStack gap="xs">
                            <Text>💰</Text>
                            <Text>{game.pricing.finalFormatted}</Text>
                        </HStack>
                    )}
                    {ratingText !== "N/A" && (
                        <HStack gap="xs">
                            <Text>⭐</Text>
                            <Text color={ratingColor}>{ratingText} - Overwhelmingly positive</Text>
                        </HStack>
                    )}
                    {game.platforms && game.platforms.length > 0 && (
                        <HStack gap="1" align="center" justify="center">
                            {game.platforms.slice(0, 4).map((p) => (
                                <PlatformIcon
                                    key={p.slug}
                                    type={p.slug}
                                    tooltip={p.name}
                                    boxSize="1.25rem"
                                />
                            ))}
                            {game.platforms.length > 4 && (
                                <Text fontSize="xs" color="fg.muted">
                                    +{game.platforms.length - 4}
                                </Text>
                            )}
                        </HStack>
                    )}
                </VStack>
            </Card.Body>

            <Card.Footer pt="sm" p={"sm"}>
                <Box
                    rounded="lg"
                    bg="warning"
                    color="white"
                    w="full"
                    textAlign="center"
                    py="1"
                    fontSize="xs"
                    fontWeight="bold"
                >
                    ⚠ Early Access
                </Box>
            </Card.Footer>
        </Card.Root>
    );
}
