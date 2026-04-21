import type { EventDto } from "@src/gen/catalogApi";
import { RouterLink } from "@src/components/YamadaLink/YamadaLink";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { formatEventDayLabel } from "@src/utils/eventDateTime";
import { Box, HStack, Image, Text, VStack } from "ui";
import { isEventEnded } from "../utils/eventsList";

type EventRowProps = {
    event: EventDto;
    now: Date;
};

export function EventRow({ event, now }: EventRowProps) {
    const start = formatEventDayLabel(event.startTimeUtc, event.timeZone);
    const end = formatEventDayLabel(event.endTimeUtc, event.timeZone);
    const dateRange = start && end ? `${start}–${end}` : (start ?? null);
    const imageUrl = getIGDBImageUrl(event.logoImageId, "logo_med");
    const isEnded = isEventEnded(event, now);

    return (
        <RouterLink
            to="/events/$eventId"
            params={{ eventId: String(event.id) }}
            preload="intent"
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <HStack
                justify="space-between"
                align={{ base: "stretch", md: "start" }}
                direction={{ base: "column", md: "row" }}
                px={{ base: "2", md: "2" }}
                py={{ base: "2", md: "3" }}
                gap={{ base: "2", md: "3" }}
                rounded="md"
                bg={isEnded ? "bg.panel" : "bg.surface"}
                borderWidth="1px"
                borderColor={isEnded ? "border.default" : "border.subtle"}
            >
                <HStack gap="3" minW="0" flex="1" w="full">
                    <Box
                        flexShrink={0}
                        w={{ base: "9", md: "10" }}
                        h={{ base: "11", md: "12" }}
                        rounded="md"
                        overflow="hidden"
                        bg="bg.subtle"
                        borderWidth="1px"
                        borderColor="border.subtle"
                    >
                        {imageUrl ? (
                            <Image
                                src={imageUrl}
                                alt={event.name}
                                w="full"
                                h="full"
                                objectFit="cover"
                            />
                        ) : (
                            <Box display="grid" placeItems="center" w="full" h="full">
                                <Text fontSize="xs" color="fg.muted" fontWeight="semibold">
                                    {event.name.slice(0, 1)}
                                </Text>
                            </Box>
                        )}
                    </Box>

                    <VStack align="start" gap="0" minW="0">
                        <Text
                            fontWeight="medium"
                            fontSize="sm"
                            lineClamp={2}
                            minW="0"
                            color={isEnded ? "fg.subtle" : "fg.base"}
                        >
                            {event.name}
                        </Text>
                        <Text fontSize="xs" color={isEnded ? "fg.muted" : "fg.subtle"}>
                            {isEnded ? "Finished" : "Upcoming"}
                        </Text>
                    </VStack>
                </HStack>

                {dateRange && (
                    <Text
                        fontSize="xs"
                        color={isEnded ? "fg.muted" : "fg.subtle"}
                        flexShrink={0}
                        alignSelf={{ base: "start", md: "center" }}
                    >
                        {dateRange}
                    </Text>
                )}
            </HStack>
        </RouterLink>
    );
}
