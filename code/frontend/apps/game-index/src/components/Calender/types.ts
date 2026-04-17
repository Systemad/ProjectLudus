export type CalendarDayCell = {
    key: string;
    dayLabel: string;
};

export type CalendarModel = {
    from: number;
    to: number;
    periodLabel: string;
    dayCells: CalendarDayCell[];
};
