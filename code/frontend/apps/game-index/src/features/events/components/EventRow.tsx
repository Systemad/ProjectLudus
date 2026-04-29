import type { EventDto } from "@src/gen/catalogApi";
import { RouterLink } from "@src/components/YamadaLink/YamadaLink";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { Box, HStack, Image, Text, VStack } from "ui";
import { isEventEnded } from "../utils/eventsList";

const monthDayFormatter = new Intl.DateTimeFormat("en-US", { month: "short", day: "2-digit" });

function formatDateRange(event: EventDto, now: Date) {
    if (!event.startTimeUtc) return "TBA";

    const start = monthDayFormatter.format(new Date(event.startTimeUtc));
    const end = event.endTimeUtc ? monthDayFormatter.format(new Date(event.endTimeUtc)) : null;

    return end ? `${start}–${end}` : start;
}

export function EventRow({ event, now }: { event: EventDto; now: Date }) {
    const imageUrl = getIGDBImageUrl(event.logoImageId, "logo_med");
    const isEnded = isEventEnded(event, now);
    const dateRange = formatDateRange(event, now);

    return (
        <RouterLink
            to="/events/$eventId"
            params={{ eventId: String(event.id) }}
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
                bg={isEnded ? "bg.subtle" : "bg.panel"}
            >
                <HStack gap="3" minW="0" flex="1" w="full">
                    <Box
                        flexShrink={0}
                        w={{ base: "9", md: "10" }}
                        h={{ base: "11", md: "12" }}
                        rounded="md"
                        overflow="hidden"
                        bg="bg.subtle"
                    >
                        {imageUrl ? (
                            <Image
                                src={imageUrl}
                                alt={event.name}
                                w="full"
                                h="full"
                                objectFit="cover"
                                loading="lazy"
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
