import { Box, Flex, Image, Text } from "ui";
import { RouterLink } from "@src/components/YamadaLink";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import type { GameSearchHit } from "@src/Typesense/utils/hits";

type CalendarGameRowProps = {
    hit: GameSearchHit;
};

export function CalendarGameRow({ hit }: CalendarGameRowProps) {
    const imageUrl = getIGDBImageUrl(hit.cover_url, "thumb");

    return (
        <Flex align="stretch" gap="sm" minW={0} minH={{ base: "10", md: "12" }}>
            <Image
                src={imageUrl}
                alt={hit.name ? `${hit.name} cover` : "Game cover"}
                w={{ base: "8", md: "10" }}
                h={{ base: "10", md: "12" }}
                objectFit="cover"
                rounded="sm"
                flexShrink={0}
                loading="lazy"
            />

            <Box minW={0} flex="1">
                <RouterLink
                    to="/games/$gameId"
                    params={{ gameId: String(hit.id) }}
                    style={{ textDecoration: "none", color: "inherit" }}
                >
                    <Text fontSize="sm" lineClamp={1}>
                        {hit.name ?? "Untitled game"}
                    </Text>
                </RouterLink>
            </Box>
        </Flex>
    );
}
