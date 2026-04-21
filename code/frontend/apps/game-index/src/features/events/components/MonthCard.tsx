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

    return (
        <Box
            borderWidth="1px"
            borderColor="border.subtle"
            rounded="xl"
            p={{ base: "3", md: "4" }}
            bg="bg.panel"
        >
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
                <Collapse open={expanded}>
                    <VStack gap="2" align="stretch">
                        <For each={group.events.slice(4)}>
                            {(event) => <EventRow key={event.id} event={event} now={now} />}
                        </For>
                    </VStack>
                </Collapse>
                {hasMore && (
                    <Box
                        as="button"
                        onClick={() => setExpanded((v) => !v)}
                        mt="2"
                        py="2"
                        w="full"
                        textAlign="center"
                        fontSize="sm"
                        color="fg.muted"
                        borderWidth="1px"
                        borderColor="border.subtle"
                        rounded="lg"
                        bg="bg.subtle"
                    >
                        {expanded ? "Show less" : "Expand all"}
                    </Box>
                )}
            </VStack>
        </Box>
    );
}
