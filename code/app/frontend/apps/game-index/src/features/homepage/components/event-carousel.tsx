import { Link } from "@tanstack/react-router";
import { Box, For, Heading, HStack, Image, Text } from "ui";

import type { EventDto } from "@src/gen/catalogApi";
import { RouterLink } from "@src/components/router-link";
import { isEventEnded } from "@src/features/event/utils/events-list";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { EventCountdown } from "./event-countdown";

function formatDateRange(event: EventDto): string {
    if (!event.startTimeUtc) return "TBA";
    const fmt = new Intl.DateTimeFormat("en-US", { month: "short", day: "numeric" });
    const start = fmt.format(new Date(event.startTimeUtc));
    const end = event.endTimeUtc ? fmt.format(new Date(event.endTimeUtc)) : null;
    return end ? `${start}–${end}` : start;
}

type Props = {
    events?: EventDto[];
};

export function EventCarousel({ events }: Props) {
    const now = new Date();

    return (
        <>
            <HStack justify="space-between" align="baseline">
                <Heading size="2xl">Upcoming Events</Heading>
                <Link to="/events" style={{ color: "inherit", fontSize: "sm" }}>
                    Browse all
                </Link>
            </HStack>
            <Box
                display="grid"
                gridTemplateColumns="repeat(auto-fit, minmax(min(260px, 100%), 1fr))"
                gap="md"
            >
                <For each={events} limit={4}>
                    {(event) => (
                        <RouterLink
                            key={event.id}
                            to="/events/$eventId"
                            params={{ eventId: String(event.id) }}
                            style={{
                                color: "inherit",
                                textDecoration: "none",
                                display: "block",
                            }}
                        >
                            <Box
                                position="relative"
                                rounded="lg"
                                overflow="hidden"
                                bg="bg.panel"
                                h="full"
                                minH="64"
                            >
                                {event.logoImageId && (
                                    <Image
                                        src={getIGDBImageUrl(event.logoImageId, "1080p")}
                                        alt=""
                                        position="absolute"
                                        inset="0"
                                        w="full"
                                        h="full"
                                        objectFit="cover"
                                    />
                                )}
                                <Box
                                    position="absolute"
                                    inset="0"
                                    bgGradient="linear(to-b, rgba(0,0,0,0.2) 0%, rgba(0,0,0,0.7) 50%, rgba(0,0,0,0.85) 100%)"
                                />
                                <Box
                                    position="relative"
                                    p="4"
                                    h="full"
                                    w="full"
                                    display="flex"
                                    flexDirection="column"
                                    alignItems="center"
                                    justifyContent="center"
                                    textAlign="center"
                                    gap="1.5"
                                >
                                    <Text
                                        fontSize="sm"
                                        textShadow="0 2px 8px rgba(0,0,0,0.95), 0 1px 2px rgba(0,0,0,0.6)"
                                    >
                                        {formatDateRange(event)}
                                    </Text>
                                    <Heading
                                        size="md"
                                        lineClamp={2}
                                        color="white"
                                        textShadow="0 2px 8px rgba(0,0,0,0.95), 0 1px 2px rgba(0,0,0,0.6)"
                                    >
                                        {event.name}
                                    </Heading>
                                    <Box>
                                        <EventCountdown
                                            targetUtc={event.startTimeUtc}
                                            started={isEventEnded(event, now)}
                                        />
                                    </Box>
                                </Box>
                            </Box>
                        </RouterLink>
                    )}
                </For>
            </Box>
        </>
    );
}
