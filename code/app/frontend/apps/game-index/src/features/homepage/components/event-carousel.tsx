import { Link } from "@tanstack/react-router";
import { HStack } from "@astryxdesign/core/HStack";
import { Text } from "@astryxdesign/core/Text";

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
            <HStack hAlign="between" style={{alignItems: "baseline"}}>
                <Text as="h2" style={{fontSize: "1.5rem"}}>Upcoming Events</Text>
                <Link to="/events" style={{ color: "inherit", fontSize: "0.875rem" }}>
                    Browse all
                </Link>
            </HStack>
            <div
                style={{
                    display: "grid",
                    gridTemplateColumns: "repeat(auto-fit, minmax(min(260px, 100%), 1fr))",
                    gap: "md",
                }}
            >
                {events?.slice(0, 4).map((event) => (
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
                        <div
                            style={{
                                position: "relative",
                                borderRadius: "var(--radius-lg)",
                                overflow: "hidden",
                                background: "var(--bg-panel)",
                                height: "100%",
                                minHeight: "16rem",
                            }}
                        >
                            {event.logoImageId && (
                                <img
                                    src={getIGDBImageUrl(event.logoImageId, "1080p")}
                                    alt=""
                                    style={{
                                        position: "absolute",
                                        inset: "0",
                                        width: "100%",
                                        height: "100%",
                                        objectFit: "cover",
                                    }}
                                />
                            )}
                            <div
                                style={{
                                    position: "absolute",
                                    inset: "0",
                                    background: "linear-gradient(to bottom, rgba(0,0,0,0.2) 0%, rgba(0,0,0,0.7) 50%, rgba(0,0,0,0.85) 100%)",
                                }}
                            />
                            <div
                                style={{
                                    position: "relative",
                                    padding: "1rem",
                                    height: "100%",
                                    width: "100%",
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "center",
                                    justifyContent: "center",
                                    textAlign: "center",
                                    gap: "0.375rem",
                                }}
                            >
                                <Text
                                    style={{fontSize: "0.875rem", textShadow: "0 2px 8px rgba(0,0,0,0.95), 0 1px 2px rgba(0,0,0,0.6)"}}
                                >
                                    {formatDateRange(event)}
                                </Text>
                                <Text
                                    as="h3"
                                    style={{
                                        color: "white",
                                        textShadow: "0 2px 8px rgba(0,0,0,0.95), 0 1px 2px rgba(0,0,0,0.6)",
                                        WebkitLineClamp: 2,
                                        overflow: "hidden",
                                        display: "-webkit-box",
                                        WebkitBoxOrient: "vertical",
                                        fontSize: "1.125rem",
                                        fontWeight: 600,
                                    }}
                                >
                                    {event.name}
                                </Text>
                                <div>
                                    <EventCountdown
                                        targetUtc={event.startTimeUtc}
                                        started={isEventEnded(event, now)}
                                    />
                                </div>
                            </div>
                        </div>
                    </RouterLink>
                ))}
            </div>
        </>
    );
}
