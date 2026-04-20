"use client";
import {
    HStack,
    Text,
    VStack,
    Container,
    For,
    ButtonGroup,
    ChevronLeftIcon,
    ChevronRightIcon,
    Float,
    Box,
} from "ui";
import { useCalendarContext } from "@src/features/calendar/hooks/useCalendarContext";
import { dayjs } from "@src/utils/dayjs";

type DayCellProps = {
    date: string;
    monthLabel: string;
    dayNum: string;
    dayName: string;
    count: number;
    isActive: boolean;
    isToday: boolean;
    onJumpToDay: (date: string) => void;
};

function DayCell({
    date,
    monthLabel,
    dayNum,
    dayName,
    count,
    isActive,
    isToday,
    onJumpToDay,
}: DayCellProps) {
    const remainingCount = Math.max(count - 2, 0);
    return (
        <VStack
            as="button"
            gap="0"
            onClick={() => onJumpToDay(date)}
            py="2"
            px="1"
            rounded="2xl"
            minW="0"
            borderWidth="1px"
            bg={isActive ? "colorScheme.subtle" : "bg.panel"}
            borderColor={
                isActive ? "colorScheme.outline" : isToday ? "border.default" : "border.subtle"
            }
            transform={isActive ? "scale(1)" : "scale(0.96)"}
        >
            <Text
                fontSize={{ base: "2xs", md: "xs" }}
                color={isActive ? "colorScheme.fg" : "fg.muted"}
            >
                {monthLabel}
            </Text>
            <Text fontSize={{ base: "lg", md: "xl" }} color={isActive ? "fg" : "fg.default"}>
                {dayNum}
            </Text>
            <Text
                fontSize={{ base: "xs", md: "sm" }}
                fontWeight="medium"
                color={isActive ? "colorScheme.fg" : "fg.default"}
            >
                {dayName}
            </Text>
            {remainingCount > 0 && (
                <Float
                    placement="end-center"
                    rounded="full"
                    px="1.5"
                    py="0.5"
                    fontSize="xs"
                    bg="bg.contrast"
                    color="fg.contrast"
                >
                    +{remainingCount}
                </Float>
            )}
        </VStack>
    );
}

type WeekOverviewHeaderProps = {
    weekStartDate: string;
    dayCounts: Record<string, number>;
};

export function WeekOverviewHeader({ weekStartDate, dayCounts }: WeekOverviewHeaderProps) {
    const { activeMonthKey, activeDayKey, onJumpToDay, onJumpToToday, onChangeWeek } =
        useCalendarContext();

    const weekStart = dayjs.utc(weekStartDate);
    const activeMonthLabel = dayjs.utc(`${activeMonthKey}-01`).format("MMMM YYYY");
    const todayKey = dayjs.utc().format("YYYY-MM-DD");

    const days = Array.from({ length: 7 }).map((_, i) => {
        const date = weekStart.add(i, "days");
        const dateKey = date.format("YYYY-MM-DD");
        return {
            date: dateKey,
            monthLabel: date.format("MMM"),
            dayName: date.format("ddd"),
            dayNum: date.format("D"),
            count: dayCounts[dateKey] ?? 0,
        };
    });

    return (
        <Container.Root position="relative" w="full" rounded="2xl" variant="panel">
            <Container.Body>
                <VStack align="stretch">
                    <HStack justify="space-between" align="center">
                        <Text
                            fontSize={{ base: "2xs", md: "xs" }}
                            color="fg.muted"
                            fontWeight="medium"
                            lineHeight="short"
                        >
                            {activeMonthLabel}
                        </Text>
                        <ButtonGroup.Root attached rounded="xl">
                            <ButtonGroup.IconItem
                                onClick={() => onChangeWeek(-1)}
                                aria-label="Previous week"
                                rounded="2xl"
                                icon={<ChevronLeftIcon />}
                            />
                            <ButtonGroup.Item onClick={onJumpToToday}>Today</ButtonGroup.Item>
                            <ButtonGroup.IconItem
                                onClick={() => onChangeWeek(1)}
                                aria-label="Next week"
                                rounded="2xl"
                                icon={<ChevronRightIcon />}
                            />
                        </ButtonGroup.Root>
                    </HStack>

                    <Box
                        display="grid"
                        gridTemplateColumns="repeat(7, minmax(0, 1fr))"
                        gap={{ base: "1", md: "1.5" }}
                        py="2"
                    >
                        <For each={days}>
                            {(day) => (
                                <DayCell
                                    key={day.date}
                                    {...day}
                                    isActive={day.date === activeDayKey}
                                    isToday={day.date === todayKey}
                                    onJumpToDay={onJumpToDay}
                                />
                            )}
                        </For>
                    </Box>
                </VStack>
            </Container.Body>
        </Container.Root>
    );
}
