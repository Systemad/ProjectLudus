"use client";

import { createContext, useContext } from "react";

export type CalendarContextValue = {
    activeMonthKey: string;
    activeDayKey: string;
    onJumpToDay: (dayKey: string) => void;
    onJumpToToday: () => void;
    onChangeWeek: (direction: -1 | 1) => void;
    registerSection: (dayKey: string, node: HTMLElement | null) => void;
};

const CalendarContext = createContext<CalendarContextValue | null>(null);

type CalendarProviderProps = {
    children: React.ReactNode;
    value: CalendarContextValue;
};

export function CalendarProvider({ children, value }: CalendarProviderProps) {
    return <CalendarContext.Provider value={value}>{children}</CalendarContext.Provider>;
}

export function useCalendarContext() {
    const context = useContext(CalendarContext);

    if (!context) {
        throw new Error("useCalendarContext must be used within CalendarProvider");
    }

    return context;
}
