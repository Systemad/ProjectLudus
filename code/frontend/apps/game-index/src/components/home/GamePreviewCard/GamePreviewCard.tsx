"use client";

import { HStack, Image, Tag, Text, VStack, Wrap, Card } from "ui";
import type { GameDto } from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { formatReleaseDateLabel } from "@src/utils/dateUtils";

type Props = {
    game: GameDto;
};

export default function GamePreviewCard({ game }: Props) {
    const title = game.name ?? "Unknown game";
    const developer = game.developers?.[0]?.name ?? "Unknown";
    const typeLabel = game.gameType ?? "Unknown";
    const tags = [...game.genres, ...game.gameModes].slice(0, 3).map((feature) => feature.name);
    const features = [...game.platforms, ...game.themes].map((feature) => feature.name);
    const featuresSummary =
        features.length > 0
            ? `${features.slice(0, 2).join(", ")}${features.length > 2 ? ` +${features.length - 2} more` : ""}`
            : "No additional features";

    const imageUrl = getIGDBImageUrl(game.coverUrl, "cover_big");
    const releaseLabel = formatReleaseDateLabel(game.firstReleaseDate);

    return (
        <Card.Root maxW="xs" rounded="lg" bg="bg.panel" boxShadow="md" border="none">
            <Card.Header justifyContent="center">
                <Image w="full" h="3xs" src={imageUrl} alt={title} objectFit="cover" rounded="md" />
            </Card.Header>
            <Card.Body p="sm">
                <VStack align="stretch" gap="sm">
                    <Text color="fg.base" fontSize="lg" fontWeight="bold" lineHeight="short">
                        {title}
                    </Text>

                    <VStack align="stretch" gap="xs">
                        <Text color="fg.subtle" fontSize="sm">
                            Developer: {""}
                            <Text as="span" color="fg.base">
                                {developer}
                            </Text>
                        </Text>
                        <Text color="fg.subtle" fontSize="sm">
                            Type: {""}
                            <Text as="span" color="fg.base">
                                {typeLabel}
                            </Text>
                        </Text>
                        <Text color="fg.subtle" fontSize="sm">
                            Released: {""}
                            <Text as="span" color="fg.base">
                                {releaseLabel}
                            </Text>
                        </Text>
                    </VStack>

                    <Wrap gap="xs">
                        {tags.map((tag) => (
                            <Tag
                                key={`${game.id ?? title}-${tag}`}
                                variant="surface"
                                colorScheme="yellow"
                                textTransform="none"
                                size="sm"
                            >
                                {tag}
                            </Tag>
                        ))}
                    </Wrap>

                    <HStack rounded="md" bg="bg.subtle" p="sm" gap="xs" align="start">
                        <Text color="fg.subtle" fontSize="sm" lineHeight="short">
                            Features: {""}
                            <Text as="span" color="fg.base">
                                {featuresSummary}
                            </Text>
                        </Text>
                    </HStack>
                </VStack>
            </Card.Body>
        </Card.Root>
    );
}
