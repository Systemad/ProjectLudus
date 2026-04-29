import type { GameDto } from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import { isTbaReleaseDate } from "@src/utils/dateUtils";
import { Link } from "@tanstack/react-router";
import { Box, Format, HStack, Image, Text, VStack } from "ui";

export function GameRow({ game, isPlaceholder = false }: { game: GameDto; isPlaceholder?: boolean }) {
    const dayLabel = isPlaceholder ? (
        "Date TBA"
    ) : !isTbaReleaseDate(game.firstReleaseDate) && game.firstReleaseDate ? (
        <Format.DateTime value={new Date(game.firstReleaseDate)} month="short" day="2-digit" year="numeric" />
    ) : (
        "TBA"
    );
    const imageUrl = game.coverUrl ? getIGDBImageUrl(game.coverUrl, "cover_small") : null;
    const gameId = String(game.id);
    const studioLabel = game.developers?.[0]?.name ?? "Upcoming release";
    const platformIcons = game.platforms?.slice(0, 4).map((platform) => platform.slug ?? platform.name) ?? [];

    return (
        <Link
            to="/games/$gameId"
            params={{ gameId }}
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <HStack align="start" px="2" py="3" gap="3" rounded="md" bg="bg.surface">
                <Box flexShrink={0} w="10" h="12" rounded="md" overflow="hidden" bg="bg.subtle">
                    {imageUrl ? (
                        <Image src={imageUrl} alt={game.name} w="full" h="full" objectFit="cover" loading="lazy" />
                    ) : (
                        <Box display="grid" placeItems="center" w="full" h="full">
                            <Text fontSize="xs" color="fg.muted" fontWeight="semibold">
                                {game.name.slice(0, 1)}
                            </Text>
                        </Box>
                    )}
                </Box>

                <VStack align="stretch" gap="1" flex="1" minW="0">
                    <HStack justify="space-between" align="flex-start" gap="3" minW="0" w="full">
                        <Text fontWeight="medium" fontSize="sm" lineClamp={2} minW="0" color="fg.base" flex="1">
                            {game.name}
                        </Text>
                        {dayLabel && (
                            <Text fontSize="xs" color="fg.subtle" textAlign="end" flexShrink={0}>
                                {dayLabel}
                            </Text>
                        )}
                    </HStack>

                    <HStack justify="space-between" align="center" gap="2" minW="0" w="full">
                        <Text fontSize="xs" color="fg.subtle" minW="0" flex="1" lineClamp={1}>
                            {studioLabel}
                        </Text>
                        <HStack gap="2" align="center" flexShrink={0}>
                            {platformIcons.map((platform) => (
                                <PlatformIcon key={platform} type={platform} />
                            ))}
                        </HStack>
                    </HStack>
                </VStack>
            </HStack>
        </Link>
    );
}
