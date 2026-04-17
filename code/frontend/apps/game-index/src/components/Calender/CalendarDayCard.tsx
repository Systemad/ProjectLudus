import { Box, For, Gamepad2Icon, Text } from "ui";
import type { GameSearchHit } from "@src/Typesense/utils/hits";
import type { CalendarDayCell } from "./types";
import { CalendarGameRow } from "./CalendarGameRow";

const MAX_VISIBLE_DAY_ENTRIES = 4;

type CalendarDayCardProps = {
    cell: CalendarDayCell;
    dayEntries: GameSearchHit[];
};

export function CalendarDayCard({ cell, dayEntries }: CalendarDayCardProps) {
    const hasEntries = dayEntries.length > 0;
    const hiddenCount = Math.max(dayEntries.length - MAX_VISIBLE_DAY_ENTRIES, 0);

    return (
        <Box
            rounded="md"
            borderWidth="1px"
            borderColor="border.subtle"
            minH={{ base: "44", md: "64" }}
            p={{ base: "xs", md: "sm" }}
            display="grid"
            gridTemplateRows="auto 1fr"
            gap="xs"
            style={{ contentVisibility: "auto", containIntrinsicSize: "280px" }}
        >
            <Text fontSize="xs" fontWeight="semibold">
                {cell.dayLabel}
            </Text>

            {hasEntries ? (
                <Box display="flex" flexDirection="column" gap={{ base: "xs", md: "sm" }} h="full">
                    <Box display="grid" gap={{ base: "xs", md: "sm" }} alignContent="start">
                        <For each={dayEntries} limit={MAX_VISIBLE_DAY_ENTRIES}>
                            {(hit) => <CalendarGameRow key={String(hit.id)} hit={hit} />}
                        </For>
                    </Box>
                    {hiddenCount > 0 ? (
                        <Text fontSize="xs" color="fg.subtle" textAlign="right" mt="auto">
                            +{hiddenCount} more
                        </Text>
                    ) : null}
                </Box>
            ) : (
                <Box
                    h={{ base: "44", md: "72" }}
                    w="full"
                    display="grid"
                    placeItems="center"
                    textAlign="center"
                    gap="xs"
                >
                    <Gamepad2Icon boxSize="5" color="fg.muted" />
                    <Text fontSize="xs" color="fg.subtle">
                        No releases this day
                    </Text>
                </Box>
            )}
        </Box>
    );
}
