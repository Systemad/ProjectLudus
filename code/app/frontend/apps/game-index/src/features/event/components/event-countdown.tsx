import { useEffect, useState } from "react";
import { parseISO } from "date-fns";
import { Text, VStack } from "ui";

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
            <VStack align="start" gap="1">
                <Text
                    fontSize="xs"
                    color="fg.muted"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    Countdown
                </Text>
                <Text fontSize="2xl" fontWeight="bold">
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
            <VStack align="start" gap="1">
                <Text
                    fontSize="xs"
                    color="fg.muted"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    Countdown
                </Text>
                <Text fontSize="lg" fontWeight="bold">
                    Event has been concluded
                </Text>
            </VStack>
        );
    }

    if (target <= now) {
        return (
            <VStack align="start" gap="1">
                <Text
                    fontSize="xs"
                    color="fg.muted"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    Countdown
                </Text>
                <Text fontSize="2xl" fontWeight="bold">
                    Started
                </Text>
            </VStack>
        );
    }

    return (
        <VStack align="start" gap="1">
            <Text fontSize="xs" color="fg.muted" textTransform="uppercase" letterSpacing="widest">
                Countdown
            </Text>
            <Text fontSize="2xl" fontWeight="bold">
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
            <VStack align="start" gap="1">
                <Text
                    fontSize="xs"
                    color="fg.muted"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    Countdown
                </Text>
                <Text fontSize="lg" fontWeight="bold">
                    TBD
                </Text>
            </VStack>
        );
    }

    if (started) {
        return (
            <VStack align="start" gap="1">
                <Text
                    fontSize="xs"
                    color="fg.muted"
                    textTransform="uppercase"
                    letterSpacing="widest"
                >
                    Countdown
                </Text>
                <Text fontSize="lg" fontWeight="bold">
                    Started
                </Text>
            </VStack>
        );
    }

    const diff = parseISO(targetUtc).getTime() - now.getTime();

    return (
        <VStack align="start" gap="1">
            <Text fontSize="xs" color="fg.muted" textTransform="uppercase" letterSpacing="widest">
                Countdown
            </Text>
            <Text fontSize="xl" fontWeight="bold">
                {formatHmsShort(diff)}
            </Text>
        </VStack>
    );
}
