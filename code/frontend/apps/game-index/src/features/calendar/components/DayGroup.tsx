"use client";
import { Box, Flex, For, Text } from "ui";
import type { DayBucket } from "@src/features/calendar/utils/calendarWeekGrouping";
import { useCalendarContext } from "@src/features/calendar/hooks/useCalendarContext";
import { MAX_GAMES_PER_DAY } from "@src/features/calendar/utils/date.utils";
import { GameCard } from "@src/components/game/GameCard";
type DayGroupProps = {
    bucket: DayBucket;
};

export function DayGroup({ bucket }: DayGroupProps) {
    const { registerSection } = useCalendarContext();

    return (
        <Box
            ref={(node) => registerSection?.(bucket.dayKey, node)}
            key={bucket.dayKey}
            data-day-key={bucket.dayKey}
            display="grid"
            gap="md"
        >
            <Flex align="center" gap="4">
                <Text flexShrink={0} fontSize={{ base: "2xl", md: "4xl" }} fontWeight="medium">
                    {bucket.headingLabel}
                </Text>
                <Box
                    flex="1"
                    borderTopWidth="1px"
                    borderTopStyle="dashed"
                    borderColor="border.subtle"
                />
            </Flex>

            {bucket.games.length === 0 ? (
                <Box
                    minH={{ base: "32", md: "40" }}
                    display="grid"
                    placeItems="center"
                    rounded="lg"
                    borderWidth="1px"
                    borderColor="border.subtle"
                    bg="bg.subtle"
                >
                    <Text color="fg.muted" fontSize="sm">
                        No releases
                    </Text>
                </Box>
            ) : (
                <Flex gap="4" overflowX="auto" overflowY="hidden" pb="2">
                    <For each={bucket.games.slice(0, MAX_GAMES_PER_DAY)}>
                        {(game) => <GameCard key={String(game.id)} game={game} />}
                    </For>

                    {bucket.games.length > MAX_GAMES_PER_DAY ? (
                        <Box
                            minW={{ base: "20", md: "24" }}
                            w={{ base: "20", md: "24" }}
                            h={{ base: "40", md: "48" }}
                            rounded="2xl"
                            borderWidth="1px"
                            borderColor="border.base"
                            bg="bg.panel"
                            display="grid"
                            placeItems="center"
                            textAlign="center"
                            px="3"
                        >
                            <Text fontSize="lg" fontWeight="medium">
                                +{bucket.games.length - MAX_GAMES_PER_DAY} more
                            </Text>
                            <Text fontSize="sm" color="fg.subtle">
                                View all
                            </Text>
                        </Box>
                    ) : null}
                </Flex>
            )}
        </Box>
    );
}
