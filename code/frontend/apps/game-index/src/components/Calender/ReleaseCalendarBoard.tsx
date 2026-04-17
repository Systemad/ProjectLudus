import { Box, Flex, Text } from "ui";
import { useHits } from "react-instantsearch";
import type { GameSearchHit } from "@src/Typesense/utils/hits";
import type { CalendarDayCell } from "./types";
import { CalendarDayCard } from "./CalendarDayCard";
import { bucketHitsByDay } from "./bucketHitsByDay";

const EMPTY_DAY_ENTRIES: GameSearchHit[] = [];

type ReleaseCalendarBoardProps = {
    dayCells: CalendarDayCell[];
};

export function ReleaseCalendarBoard({ dayCells }: ReleaseCalendarBoardProps) {
    const { items } = useHits<GameSearchHit>();
    const entriesByDay = bucketHitsByDay(items);

    return (
        <Box display="grid" gap={{ base: "sm", md: "md" }}>
            <Flex
                display={{ base: "none", md: "grid" }}
                gridTemplateColumns="repeat(7, minmax(0, 1fr))"
                gap="md"
            >
                {["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"].map((weekday) => (
                    <Text key={weekday} fontSize="sm" fontWeight="semibold" textAlign="center">
                        {weekday}
                    </Text>
                ))}
            </Flex>

            <Box
                display="grid"
                gridTemplateColumns={{ base: "1fr", md: "repeat(7, minmax(0, 1fr))" }}
                gap={{ base: "sm", md: "md" }}
            >
                {dayCells.map((cell) => (
                    <CalendarDayCard
                        key={cell.key}
                        cell={cell}
                        dayEntries={entriesByDay.get(cell.key) ?? EMPTY_DAY_ENTRIES}
                    />
                ))}
            </Box>
        </Box>
    );
}
