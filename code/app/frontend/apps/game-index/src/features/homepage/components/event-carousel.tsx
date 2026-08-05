import { Link } from "@tanstack/react-router";
import { HStack } from "@astryxdesign/core/HStack";
import { Text } from "@astryxdesign/core/Text";
import { AspectRatio } from "@astryxdesign/core/AspectRatio";
import { MediaTheme } from "@astryxdesign/core/theme";
import { Overlay } from "@astryxdesign/core/Overlay";
import * as stylex from "@stylexjs/stylex";

import type { EventDto } from "@src/gen/catalogApi";
import { RouterLink } from "@src/components/router-link";
import { isEventEnded } from "@src/features/event/utils/events-list";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { EventCountdown } from "./event-countdown";

const styles = stylex.create({
    header: {
        alignItems: "baseline",
    },
    heading: {
        fontSize: "1.5rem",
    },
    browseLink: {
        color: "inherit",
        fontSize: "0.875rem",
    },
    grid: {
        display: "grid",
        gridTemplateColumns: "repeat(auto-fit, minmax(min(260px, 100%), 1fr))",
        gap: "var(--spacing-3)",
    },
    link: {
        display: "block",
        color: "inherit",
        textDecoration: "none",
    },
    content: {
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        flexDirection: "column",
        gap: "var(--spacing-1)",
        width: "100%",
        padding: "var(--spacing-4)",
        textAlign: "center",
    },
    title: {
        display: "-webkit-box",
        overflow: "hidden",
        WebkitBoxOrient: "vertical",
        WebkitLineClamp: 2,
        fontSize: "1.125rem",
    },
    media: {
        overflow: "hidden",
        borderRadius: "var(--radius-container)",
        backgroundColor: "var(--color-background-surface)",
    },
    image: {
        width: "100%",
        height: "100%",
        objectFit: "cover",
    },
});

function formatDateRange(event: EventDto): string {
    if (!event.startTimeUtc) return "TBA";
    const fmt = new Intl.DateTimeFormat("en-US", { month: "short", day: "numeric" });
    const start = fmt.format(new Date(event.startTimeUtc));
    const end = event.endTimeUtc ? fmt.format(new Date(event.endTimeUtc)) : null;
    return end ? `${start} - ${end}` : start;
}

type Props = {
    events?: EventDto[];
};

export function EventCarousel({ events }: Props) {
    const now = new Date();

    return (
        <>
            <HStack hAlign="between" xstyle={styles.header}>
                <Text as="h2" xstyle={styles.heading}>Upcoming Events</Text>
                <Link to="/events" {...stylex.props(styles.browseLink)}>
                    Browse all
                </Link>
            </HStack>
            <div {...stylex.props(styles.grid)}>
                {events?.slice(0, 4).map((event) => (
                    <RouterLink
                        key={event.id}
                        to="/events/$eventId"
                        params={{ eventId: String(event.id) }}
                        {...stylex.props(styles.link)}
                    >
                        <Overlay
                            position="fill"
                            align="center"
                            content={
                                <MediaTheme mode="dark">
                                    <div {...stylex.props(styles.content)}>
                                        <Text type="supporting">{formatDateRange(event)}</Text>
                                        <Text
                                            as="h3"
                                            weight="semibold"
                                            xstyle={styles.title}
                                        >
                                            {event.name}
                                        </Text>
                                        <EventCountdown
                                            targetUtc={event.startTimeUtc}
                                            started={isEventEnded(event, now)}
                                        />
                                    </div>
                                </MediaTheme>
                            }
                        >
                            <AspectRatio
                                ratio={16 / 9}
                                xstyle={styles.media}
                            >
                                {event.logoImageId && (
                                    <img
                                        src={getIGDBImageUrl(event.logoImageId, "1080p")}
                                        alt=""
                                        {...stylex.props(styles.image)}
                                    />
                                )}
                            </AspectRatio>
                        </Overlay>
                    </RouterLink>
                ))}
            </div>
        </>
    );
}
