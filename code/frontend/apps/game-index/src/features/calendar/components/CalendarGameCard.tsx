import { Box, Flex, Image, Text } from "ui";
import { RouterLink } from "@src/components/YamadaLink/YamadaLink";
import type { GamesSearchDto } from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type CalendarGameCardProps = {
    game: GamesSearchDto;
};

export function CalendarGameCard({ game }: CalendarGameCardProps) {
    const imageUrl = game.coverUrl ? getIGDBImageUrl(game.coverUrl, "cover_big") : undefined;

    return (
        <RouterLink
            key={String(game.id)}
            to="/games/$gameId"
            params={{ gameId: String(game.id) }}
            style={{ textDecoration: "none" }}
        >
            <Box
                position="relative"
                minW={{ base: "28", md: "32" }}
                w={{ base: "28", md: "32" }}
                h={{ base: "40", md: "48" }}
                rounded="2xl"
                overflow="hidden"
                borderWidth="1px"
                borderColor="border.base"
                bg="bg.panel"
            >
                {imageUrl ? (
                    <Image
                        src={imageUrl}
                        alt={game.name}
                        w="full"
                        h="full"
                        objectFit="cover"
                        loading="lazy"
                    />
                ) : (
                    <Flex w="full" h="full" align="center" justify="center" p="4">
                        <Text textAlign="center" fontSize="sm" color="fg.muted">
                            {game.name}
                        </Text>
                    </Flex>
                )}

                <Box
                    position="absolute"
                    insetX="0"
                    bottom="0"
                    bgGradient="linear(to-t, blackAlpha.900, blackAlpha.500, transparent)"
                    px="3"
                    py="3"
                >
                    <Text fontSize="sm" fontWeight="medium" color="fg.contrast" lineClamp={2}>
                        {game.name}
                    </Text>
                </Box>
            </Box>
        </RouterLink>
    );
}
