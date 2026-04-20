import { useCallback, useEffect, useRef, useState } from "react";
import { dayjs } from "@src/utils/dayjs";

const DEFAULT_TOP_OFFSET = 96;
const ACTIVE_ANCHOR_OFFSET = 8;

type UseCalendarMonthObserverOptions = {
    topOffsetPx?: number;
};

export function useCalendarMonthObserver(options?: UseCalendarMonthObserverOptions) {
    const topOffsetPx = options?.topOffsetPx ?? DEFAULT_TOP_OFFSET;
    const [activeMonthKey, setActiveMonthKey] = useState(() => dayjs.utc().format("YYYY-MM"));
    const [activeDayKey, setActiveDayKey] = useState(() => dayjs.utc().format("YYYY-MM-DD"));

    const sectionMapRef = useRef(new Map<string, HTMLElement>());
    const rafRef = useRef<number | null>(null);

    const updateActiveDayFromLayout = useCallback(() => {
        const sections = Array.from(sectionMapRef.current.entries())
            .filter(([, node]) => node.isConnected)
            .map(([dayKey, node]) => ({
                dayKey,
                nodeTop: node.offsetTop,
            }))
            .sort((left, right) => left.nodeTop - right.nodeTop);

        if (sections.length === 0) return;

        const anchorY = window.scrollY + topOffsetPx + ACTIVE_ANCHOR_OFFSET;
        let candidateDayKey = sections[0].dayKey;

        for (const section of sections) {
            if (section.nodeTop <= anchorY) {
                candidateDayKey = section.dayKey;
                continue;
            }

            break;
        }

        const monthKey = dayjs.utc(candidateDayKey).format("YYYY-MM");
        setActiveMonthKey((current) => (current === monthKey ? current : monthKey));
        setActiveDayKey((current) => (current === candidateDayKey ? current : candidateDayKey));
    }, [topOffsetPx]);

    const scheduleActiveDayRecalc = useCallback(() => {
        if (rafRef.current !== null) return;

        rafRef.current = window.requestAnimationFrame(() => {
            rafRef.current = null;
            updateActiveDayFromLayout();
        });
    }, [updateActiveDayFromLayout]);

    const registerSection = useCallback(
        (dayKey: string, node: HTMLElement | null) => {
            if (!node) {
                sectionMapRef.current.delete(dayKey);
                scheduleActiveDayRecalc();
                return;
            }

            const currentNode = sectionMapRef.current.get(dayKey);
            if (currentNode === node) return;

            sectionMapRef.current.set(dayKey, node);

            scheduleActiveDayRecalc();
        },
        [scheduleActiveDayRecalc],
    );

    const getSectionNode = useCallback((dayKey: string) => {
        return sectionMapRef.current.get(dayKey) ?? null;
    }, []);

    useEffect(() => {
        const handleScroll = () => scheduleActiveDayRecalc();
        const handleResize = () => scheduleActiveDayRecalc();
        window.addEventListener("scroll", handleScroll, { passive: true });
        window.addEventListener("resize", handleResize);
        scheduleActiveDayRecalc();

        return () => {
            window.removeEventListener("scroll", handleScroll);
            window.removeEventListener("resize", handleResize);

            if (rafRef.current !== null) {
                window.cancelAnimationFrame(rafRef.current);
                rafRef.current = null;
            }
        };
    }, [scheduleActiveDayRecalc]);

    return {
        activeMonthKey,
        activeDayKey,
        getSectionNode,
        registerSection,
    };
}
