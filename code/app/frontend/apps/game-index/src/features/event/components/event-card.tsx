import type { EventDto } from "@src/gen/catalogApi";
import { RouterLink } from "@src/components/router-link";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { Text } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { isEventEnded } from "../utils/events-list";

const monthDayFormatter = new Intl.DateTimeFormat("en-US", { month: "short", day: "2-digit" });

function formatDateRange(event: EventDto) {
    if (!event.startTimeUtc) return "TBA";

    const start = monthDayFormatter.format(new Date(event.startTimeUtc));
    const end = event.endTimeUtc ? monthDayFormatter.format(new Date(event.endTimeUtc)) : null;

    return end ? `${start}–${end}` : start;
}

export function EventRow({ event, now }: { event: EventDto; now: Date }) {
    const imageUrl = getIGDBImageUrl(event.logoImageId, "logo_med");
    const isEnded = isEventEnded(event, now);
    const dateRange = formatDateRange(event);

    return (
        <RouterLink
            to="/events/$eventId"
            params={{ eventId: String(event.id) }}
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <HStack
                hAlign="between"
                vAlign="start"
                gap={3}
                style={{
                    flexDirection: "column",
                    paddingLeft: "0.5rem",
                    paddingRight: "0.5rem",
                    paddingTop: "0.5rem",
                    paddingBottom: "0.75rem",
                    borderRadius: "var(--radius-md)",
                    background: isEnded ? "var(--bg-subtle)" : "var(--bg-panel)",
                }}
            >
                <HStack gap={3} style={{minWidth: 0, flex: 1, width: "100%"}}>
                    <div
                        style={{
                            flexShrink: 0,
                            width: "2.25rem",
                            height: "2.75rem",
                            borderRadius: "var(--radius-md)",
                            overflow: "hidden",
                            background: "var(--bg-subtle)",
                        }}
                    >
                        {imageUrl ? (
                            <img
                                src={imageUrl}
                                alt={event.name}
                                style={{ width: "100%", height: "100%", objectFit: "cover" }}
                                loading="lazy"
                            />
                        ) : (
                            <div style={{ display: "grid", placeItems: "center", width: "100%", height: "100%" }}>
                            <Text color="secondary" weight="semibold" style={{fontSize: "0.75rem"}}>
                                {event.name.slice(0, 1)}
                            </Text>
                            </div>
                        )}
                    </div>

                    <VStack hAlign="start" gap={0} style={{minWidth: 0}}>
                        <Text
                            weight="medium"
                            maxLines={2}
                            style={{fontSize: "0.875rem", minWidth: 0, color: isEnded ? "var(--fg-subtle)" : "var(--fg-base)"}}
                        >
                            {event.name}
                        </Text>
                        <Text style={{fontSize: "0.75rem", color: isEnded ? "var(--fg-muted)" : "var(--fg-subtle)"}}>
                            {isEnded ? "Finished" : "Upcoming"}
                        </Text>
                    </VStack>
                </HStack>

                {dateRange && (
                    <Text
                        style={{ fontSize: "0.75rem", flexShrink: 0, color: isEnded ? "var(--color-text-tertiary)" : "var(--color-text-secondary)" }}
                    >
                        {dateRange}
                    </Text>
                )}
            </HStack>
        </RouterLink>
    );
}
