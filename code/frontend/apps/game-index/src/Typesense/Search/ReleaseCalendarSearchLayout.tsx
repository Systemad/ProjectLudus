import { startTransition, useState } from "react";
import { Accordion, Box, Button, Collapse, Container, Flex, Text, useBoolean } from "ui";
import { Configure, Stats } from "react-instantsearch";
import { SearchFacetFilterGroup } from "./SearchFacetFilterGroup";
import { dayjs } from "@src/utils/dayjs";
import type { ReleaseDatePreset } from "@src/utils/dateUtils";
import { PeriodNavigator } from "@src/components/Calender/PeriodNavigator";
import { ReleaseCalendarBoard } from "@src/components/Calender/ReleaseCalendarBoard";
import {
    alignAnchorToPreset,
    getCalendarModel,
    shiftAnchor,
} from "@src/components/Calender/calendarModel";
import { ReleaseDatePresetControl } from "@src/components/Calender/ReleaseDatePresetControl";
import { MinimumRatingControl } from "@src/components/Calender/MinimumRatingControl";
import { PlatformFilterGroup } from "@src/components/Calender/PlatformFilterGroup";

type ReleaseCalendarSearchLayoutProps = Record<string, never>;

export function ReleaseCalendarSearchLayout(_: ReleaseCalendarSearchLayoutProps) {
    const [datePreset, setDatePreset] = useState<ReleaseDatePreset>("month");
    const [minimumRating, setMinimumRating] = useState(0);
    const [anchorDate, setAnchorDate] = useState(() =>
        dayjs.utc().startOf("day").format("YYYY-MM-DD"),
    );
    const [isMoreFiltersOpen, moreFilters] = useBoolean();

    const calendar = getCalendarModel(datePreset, anchorDate);
    const rangeStart = Math.trunc(calendar.from);
    const rangeEnd = Math.trunc(calendar.to);
    const filters = [`first_release_date>=${rangeStart}`, `first_release_date<=${rangeEnd}`];
    if (minimumRating > 0) filters.push(`aggregated_rating>=${minimumRating}`);

    return (
        <Box display="grid" gap={{ base: "lg", md: "xl" }} py={{ base: "md", md: "xl" }}>
            <Configure hitsPerPage={100} numericFilters={filters} />

            <Container.Root rounded="xl" variant="surface">
                <Container.Body p={{ base: "md", md: "xl" }}>
                    <Box display="grid" gap={{ base: "md", md: "lg" }}>
                        <Flex
                            align={{ base: "start", lg: "center" }}
                            justify="space-between"
                            gap={{ base: "sm", md: "md" }}
                            direction={{ base: "column", lg: "row" }}
                        >
                            <Box>
                                <Text fontSize="xs" color="fg.subtle">
                                    Release calendar controls
                                </Text>
                                <Text fontSize={{ base: "md", md: "lg" }} fontWeight="semibold">
                                    Navigate release windows quickly
                                </Text>
                            </Box>
                            <Box
                                rounded="md"
                                borderWidth="1px"
                                borderColor="border.subtle"
                                px="sm"
                                py="xs"
                            >
                                <Stats />
                            </Box>
                        </Flex>

                        <Box
                            display="grid"
                            gridTemplateColumns={{
                                base: "1fr",
                                lg: "minmax(14rem, 1fr) minmax(20rem, 1.3fr) minmax(12rem, 0.8fr) auto",
                            }}
                            gap={{ base: "sm", md: "md" }}
                            alignItems="end"
                        >
                            <Box>
                                <Text fontSize="xs" mb="xs">
                                    Release Window
                                </Text>
                                <ReleaseDatePresetControl
                                    value={datePreset}
                                    onChange={(nextPreset) => {
                                        startTransition(() => {
                                            setDatePreset(nextPreset);
                                            setAnchorDate((prev) =>
                                                alignAnchorToPreset(prev, nextPreset),
                                            );
                                        });
                                    }}
                                />
                            </Box>

                            <Box>
                                <Text fontSize="xs" mb="xs">
                                    Period
                                </Text>
                                <PeriodNavigator
                                    periodLabel={calendar.periodLabel}
                                    onPrevious={() => {
                                        startTransition(() => {
                                            setAnchorDate((prev) =>
                                                shiftAnchor(prev, datePreset, -1),
                                            );
                                        });
                                    }}
                                    onNext={() => {
                                        startTransition(() => {
                                            setAnchorDate((prev) =>
                                                shiftAnchor(prev, datePreset, 1),
                                            );
                                        });
                                    }}
                                />
                            </Box>

                            <Box>
                                <Text fontSize="xs" mb="xs">
                                    Minimum Rating
                                </Text>
                                <MinimumRatingControl
                                    value={minimumRating}
                                    onChange={setMinimumRating}
                                />
                            </Box>

                            <Button
                                size="sm"
                                variant="outline"
                                onClick={moreFilters.toggle}
                                w={{ base: "full", lg: "auto" }}
                            >
                                {isMoreFiltersOpen ? "Hide filters" : "More filters"}
                            </Button>
                        </Box>

                        <Collapse open={isMoreFiltersOpen}>
                            <Box pt="sm" borderTopWidth="1px" borderColor="border.subtle">
                                <Accordion.Root multiple defaultIndex={[0, 1]}>
                                    <SearchFacetFilterGroup
                                        title="Game Type"
                                        attribute="game_type"
                                        index={0}
                                    />
                                    <PlatformFilterGroup title="Platforms" index={1} />
                                </Accordion.Root>
                            </Box>
                        </Collapse>
                    </Box>
                </Container.Body>
            </Container.Root>

            <ReleaseCalendarBoard dayCells={calendar.dayCells} />
        </Box>
    );
}
