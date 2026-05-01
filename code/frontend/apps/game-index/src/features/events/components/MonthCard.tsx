import { useState } from "react";
import { Box, Collapse, EmptyState, For, Text, VStack } from "ui";
import type { MonthGroup } from "../utils/eventsList";
import { EventRow } from "./EventRow";

type MonthCardProps = {
    group: MonthGroup;
    now: Date;
};

export function MonthCard({ group, now }: MonthCardProps) {
    const [expanded, setExpanded] = useState(false);
    const hasMore = group.events.length > 4;
    const groupId = `event-group-${group.month.replace(/\s+/g, "-")}`;

    return (
        <Box rounded="xl" p={{ base: "3", md: "4" }} bg="bg.panel" h="full">
            <Text fontWeight="semibold" fontSize="lg" mb="2">
                {group.month}
            </Text>
            <VStack gap="2" align="stretch">
                <For
                    each={group.events.slice(0, 4)}
                    fallback={<EmptyState.Root description="No events yet for this month." />}
                >
                    {(event) => <EventRow key={event.id} event={event} now={now} />}
                </For>
                <Collapse open={expanded} id={groupId}>
                    <VStack gap="2" align="stretch">
                        <For each={group.events.slice(4)}>
                            {(event) => <EventRow key={event.id} event={event} now={now} />}
                        </For>
                    </VStack>
                </Collapse>
                {hasMore && (
                    <Box
                        as="button"
                        type="button"
                        onClick={() => setExpanded((v) => !v)}
                        aria-expanded={expanded}
                        aria-controls={groupId}
                        mt="2"
                        py="2"
                        w="full"
                        textAlign="center"
                        fontSize="sm"
                        color="fg.muted"
                        bg="bg.subtle"
                        rounded="lg"
                        _hover={{ bg: "rgba(255,255,255,0.08)" }}
                    >
                        {expanded ? "Show less" : "Expand all"}
                    </Box>
                )}
            </VStack>
        </Box>
    );
}
