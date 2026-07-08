import { Card } from "@astryxdesign/core/Card";
import { Heading, Text } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import type { GameBrowseDto } from "@src/gen/catalogApi";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { steamReviewColor, steamReviewRating } from "@src/utils/SteamReviewUtils";

type Props = {
    game: GameBrowseDto;
};

export function GamePreviewCard({ game }: Props) {
    const coverSrc = game.steam?.headerUrl ?? getIGDBImageUrl(game.coverUrl, "1080p");
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
        <Card width="100%">
            <img
                src={coverSrc}
                alt={game.name}
                style={{ width: "100%", borderRadius: "0.75rem", aspectRatio: "16 / 9", objectFit: "cover" }}
            />

            <Heading level={4} maxLines={1}>
                {game.name}
            </Heading>

            <VStack hAlign="stretch" gap={1} style={{fontSize: "0.875rem"}}>
                {players && (
                    <HStack gap={2}>
                        <Text>👥</Text>
                        <Text>{players} current players</Text>
                    </HStack>
                )}
                {releaseDate && (
                    <HStack gap={2}>
                        <Text>📅</Text>
                        <Text>{releaseDate}</Text>
                    </HStack>
                )}
                {game.pricing?.finalFormatted && (
                    <HStack gap={2}>
                        <Text>💰</Text>
                        <Text>{game.pricing.finalFormatted}</Text>
                    </HStack>
                )}
                {ratingText !== "N/A" && (
                    <HStack gap={2}>
                        <Text>⭐</Text>
                        <Text style={{color: ratingColor}}>{ratingText} - Overwhelmingly positive</Text>
                    </HStack>
                )}
                {game.platforms && game.platforms.length > 0 && (
                    <HStack gap={1} vAlign="center" hAlign="center">
                        {game.platforms.slice(0, 4).map((p) => (
                            <PlatformIcon
                                key={p.slug}
                                type={p.slug}
                                tooltip={p.name}
                                boxSize="1.25rem"
                            />
                        ))}
                        {game.platforms.length > 4 && (
                            <Text color="secondary" style={{fontSize: "0.75rem"}}>
                                +{game.platforms.length - 4}
                            </Text>
                        )}
                    </HStack>
                )}
            </VStack>

            <div
                style={{
                    borderRadius: "0.5rem",
                    backgroundColor: "var(--color-warning)",
                    color: "white",
                    width: "100%",
                    textAlign: "center",
                    paddingTop: "0.25rem",
                    paddingBottom: "0.25rem",
                    fontSize: "0.75rem",
                    fontWeight: "bold",
                }}
            >
                ⚠ Early Access
            </div>
        </Card>
    );
}
