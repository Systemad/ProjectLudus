"use client";
import { Suspense } from "react";
import { VStack, Box, For } from "ui";
import { WeekSectionContainer } from "@src/features/calendar/components/WeekSectionContainer";
import { IntersectionSentinel } from "@src/features/calendar/components/IntersectionSentinel";
import { CalendarTopHeader } from "@src/features/calendar/components/CalendarTopHeader";
import { CalendarProvider } from "@src/features/calendar/hooks/useCalendarContext";
import { useVerticalCalendarController } from "@src/features/calendar/hooks/useVerticalCalendarController";

export function VerticalCalendar() {
    const { contextValue, onAddNextWeek, stickyHeaderRef, weeks } = useVerticalCalendarController();

    return (
        <CalendarProvider value={contextValue}>
            <VStack gap="0" minH="screen">
                <Box
                    ref={stickyHeaderRef}
                    w="2xl"
                    position="sticky"
                    top={{ base: "24", md: "28" }}
                    zIndex="nappa"
                    px={{ base: "2", md: "5" }}
                    pt={{ base: "1", md: "3" }}
                    pb={{ base: "1", md: "2" }}
                >
                    <Suspense fallback={<Box minH={{ base: "36", md: "40" }} />}>
                        <CalendarTopHeader />
                    </Suspense>
                </Box>

                <VStack
                    align="stretch"
                    gap="0"
                    flex="1"
                    position="relative"
                    zIndex="yamcha"
                    px={{ base: "4", md: "5" }}
                    pt={{ base: "3", md: "4" }}
                >
                    <For each={weeks}>
                        {(weekStartDate) => (
                            <Suspense key={weekStartDate} fallback={<Box minH="40" />}>
                                <WeekSectionContainer weekStartDate={weekStartDate} />
                            </Suspense>
                        )}
                    </For>
                    <Box pt={{ base: "lg", md: "2xl" }} pb={{ base: "lg", md: "2xl" }}>
                        <IntersectionSentinel onVisible={onAddNextWeek} />
                    </Box>
                </VStack>
            </VStack>
        </CalendarProvider>
    );
}
