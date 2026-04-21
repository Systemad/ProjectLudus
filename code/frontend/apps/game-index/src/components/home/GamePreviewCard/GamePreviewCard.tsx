"use client";

import { HStack, Image, Tag, Text, VStack, Wrap, Card } from "ui";
import type { GamesSearchDto } from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Props = {
    game: GamesSearchDto;
};

export default function GamePreviewCard({ game }: Props) {
    const title = game.name ?? "Unknown game";
    const developer = game.developers.length > 0 ? game.developers[0] : "Unknown";
    const publisher = game.publishers.length > 0 ? game.publishers[0] : "Unknown";
    const tags = [...game.genres, ...game.gameModes].slice(0, 3);
    const features = [...game.playerPerspectives, ...game.platforms, ...game.multiplayerModes];
    const featuresSummary =
        features.length > 0
            ? `${features.slice(0, 2).join(", ")}${features.length > 2 ? ` +${features.length - 2} more` : ""}`
            : "No additional features";

    const imageUrl = getIGDBImageUrl(game.coverUrl, "cover_big");

    return (
        <Card.Root
            maxW="xs"
            rounded="lg"
            bg="bg.panel"
            borderWidth="1px"
            borderColor="border.default"
            boxShadow="md"
        >
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
                            Publisher: {""}
                            <Text as="span" color="fg.base">
                                {publisher}
                            </Text>
                        </Text>
                        <Text color="fg.subtle" fontSize="sm">
                            Released: {""}
                            <Text as="span" color="fg.base">
                                {game.firstReleaseDate}
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
                        rounded="md"
                        bg="bg.subtle"
                        borderWidth="1px"
                        borderColor="border.subtle"
                        p="sm"
                        gap="xs"
                        align="start"
                    >
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
