"use client";

import { HStack, Image, Tag, Text, VStack, Wrap, Card } from "ui";
import type { GamesSearch } from "@src/gen/catalogApi";
import { formatReleaseDate } from "@src/utils/formatReleaseDate";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Props = {
    game: GamesSearch;
};

export default function HoveredGameCard({ game }: Props) {
    const title = game.name ?? "Unknown game";
    const releaseDate = formatReleaseDate(game.firstReleaseDate ?? null);
    const developer = game.developers?.[0] ?? "Unknown";
    const publisher = game.publishers?.[0] ?? "Unknown";
    const tags = [...(game.genres ?? []), ...(game.gameModes ?? [])].slice(0, 3);
    const features = [
        ...(game.playerPerspectives ?? []),
        ...(game.platforms ?? []),
        ...(game.multiplayerModes ?? []),
    ];
    const featuresSummary =
        features.length > 0
            ? `${features.slice(0, 2).join(", ")}${features.length > 2 ? ` +${features.length - 2} more` : ""}`
            : "No additional features";

    const imageUrl = getIGDBImageUrl(game.coverUrl, "cover_big");

    return (
        <Card.Root maxW="sm" rounded="xl">
            <Card.Header justifyContent="center">
                <Image w="full" h="2xs" src={imageUrl} alt={title} objectFit="cover" rounded="xl" />
            </Card.Header>
            <Card.Body>
                <VStack align="stretch" gap="md">
                    <Text fontSize="xl" fontWeight="bold" lineHeight="short">
                        {title}
                    </Text>

                    <VStack align="stretch" gap="xs">
                        <Text color="fg.subtle" fontSize="md">
                            Developer: {""}
                            <Text as="span" color="blue.400">
                                {developer}
                            </Text>
                        </Text>
                        <Text color="fg.subtle" fontSize="md">
                            Publisher: {""}
                            <Text as="span" color="blue.400">
                                {publisher}
                            </Text>
                        </Text>
                        <Text color="fg.subtle" fontSize="md">
                            <Text as="span" color="fg.base">
                                {releaseDate}
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

                    <HStack
                        rounded="lg"
                        bg="bg.panel"
                        borderWidth="1px"
                        borderColor="border.subtle"
                        p="md"
                        gap="sm"
                        align="start"
                    >
                        <Text color="fg.subtle" fontSize="md" lineHeight="short">
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
