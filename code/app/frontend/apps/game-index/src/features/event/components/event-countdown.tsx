import { useEffect, useState } from "react";
import { parseISO } from "date-fns";
import { Text } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";

export function CountdownClock({
    startUtc,
    endUtc,
}: {
    startUtc: string | null | undefined;
    endUtc: string | null | undefined;
}) {
    const [now, setNow] = useState(() => new Date());

    useEffect(() => {
        const timer = window.setInterval(() => setNow(new Date()), 1000);
        return () => window.clearInterval(timer);
    }, []);

    if (!startUtc) {
        return (
            <VStack hAlign="start" gap={1}>
                <Text
                    color="secondary"
                    style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                >
                    Countdown
                </Text>
                <Text weight="bold" style={{fontSize: "1.5rem"}}>
                    TBD
                </Text>
            </VStack>
        );
    }

    const target = parseISO(startUtc);
    const endTarget = endUtc ? parseISO(endUtc) : null;

    const formatHms = (ms: number) => {
        const totalSeconds = Math.max(Math.floor(ms / 1000), 0);
        const hours = Math.floor(totalSeconds / 3600);
        const minutes = Math.floor((totalSeconds % 3600) / 60);
        const seconds = totalSeconds % 60;

        return `${String(hours).padStart(2, "0")}:${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`;
    };

    if (endTarget && endTarget <= now) {
        return (
            <VStack hAlign="start" gap={1}>
                <Text
                    color="secondary"
                    style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                >
                    Countdown
                </Text>
                <Text weight="bold" style={{fontSize: "1.125rem"}}>
                    Event has been concluded
                </Text>
            </VStack>
        );
    }

    if (target <= now) {
        return (
            <VStack hAlign="start" gap={1}>
                <Text
                    color="secondary"
                    style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                >
                    Countdown
                </Text>
                <Text weight="bold" style={{fontSize: "1.5rem"}}>
                    Started
                </Text>
            </VStack>
        );
    }

    return (
        <VStack hAlign="start" gap={1}>
            <Text color="secondary" style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}>
                Countdown
            </Text>
            <Text weight="bold" style={{fontSize: "1.5rem"}}>
                {formatHms(target.getTime() - now.getTime())}
            </Text>
        </VStack>
    );
}

const formatHmsShort = (ms: number) => {
    const totalSeconds = Math.max(Math.floor(ms / 1000), 0);
    const d = Math.floor(totalSeconds / 86400);
    const h = Math.floor((totalSeconds % 86400) / 3600);
    const m = Math.floor((totalSeconds % 3600) / 60);
    const s = totalSeconds % 60;
    return `${String(d).padStart(2, "0")} : ${String(h).padStart(2, "0")} : ${String(m).padStart(2, "0")} : ${String(s).padStart(2, "0")}`;
};

export function EventCountdown({
    targetUtc,
    started,
}: {
    targetUtc: string | null | undefined;
    started: boolean;
}) {
    const [now, setNow] = useState(() => new Date());

    useEffect(() => {
        const timer = window.setInterval(() => setNow(new Date()), 1000);
        return () => window.clearInterval(timer);
    }, []);

    if (!targetUtc) {
        return (
            <VStack hAlign="start" gap={1}>
                <Text
                    color="secondary"
                    style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                >
                    Countdown
                </Text>
                <Text weight="bold" style={{fontSize: "1.125rem"}}>
                    TBD
                </Text>
            </VStack>
        );
    }

    if (started) {
        return (
            <VStack hAlign="start" gap={1}>
                <Text
                    color="secondary"
                    style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}
                >
                    Countdown
                </Text>
                <Text weight="bold" style={{fontSize: "1.125rem"}}>
                    Started
                </Text>
            </VStack>
        );
    }

    const diff = parseISO(targetUtc).getTime() - now.getTime();

    return (
        <VStack hAlign="start" gap={1}>
            <Text color="secondary" style={{fontSize: "0.75rem", textTransform: "uppercase", letterSpacing: "0.25em"}}>
                Countdown
            </Text>
            <Text weight="bold" style={{fontSize: "1.25rem"}}>
                {formatHmsShort(diff)}
            </Text>
        </VStack>
    );
}
