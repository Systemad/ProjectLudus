import { useState } from "react";
import { Card } from "@astryxdesign/core/Card";
import { Text } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";
import { EmptyState } from "@astryxdesign/core/EmptyState";
import { EventRow } from "./event-card";
import type { MonthGroup } from "../utils/events-list";

type MonthCardProps = {
    group: MonthGroup;
    now: Date;
};

export function MonthCard({ group, now }: MonthCardProps) {
    const [expanded, setExpanded] = useState(false);
    const hasMore = group.events.length > 4;
    const groupId = `event-group-${group.month.replace(/\s+/g, "-")}`;

    return (
        <Card padding={3} style={{height: "100%"}}>
            <Text weight="semibold" style={{fontSize: "1.125rem", marginBottom: "0.5rem"}}>
                {group.month}
            </Text>
                    <VStack gap={2} hAlign="stretch">
                {group.events.length > 0
                    ? group.events.slice(0, 4).map((event) => (
                        <EventRow key={event.id} event={event} now={now} />
                      ))
                    : <EmptyState title="No events" description="No events yet for this month." />
                }
                {expanded && (
            <VStack gap={2} hAlign="stretch">
                        {group.events.slice(4).map((event) => (
                            <EventRow key={event.id} event={event} now={now} />
                        ))}
                    </VStack>
                )}
                {hasMore && (
                    <div
                        onClick={() => setExpanded((v) => !v)}
                        aria-expanded={expanded}
                        aria-controls={groupId}
                        style={{
                            marginTop: "0.5rem",
                            paddingTop: "0.5rem",
                            paddingBottom: "0.5rem",
                            width: "100%",
                            textAlign: "center",
                            fontSize: "0.875rem",
                            color: "var(--fg-muted)",
                            background: "var(--bg-subtle)",
                            borderRadius: "var(--radius-lg)",
                            cursor: "pointer",
                        }}
                    >
                        {expanded ? "Show less" : "Expand all"}
                    </div>
                )}
            </VStack>
        </Card>
    );
}
