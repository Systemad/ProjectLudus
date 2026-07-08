import { Text } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";

function computeRemaining(targetUtc: string) {
    const diff = new Date(targetUtc).getTime() - Date.now();
    if (diff <= 0) return { days: 0, hours: 0, minutes: 0 };
    const totalMinutes = Math.floor(diff / 60_000);
    return {
        days: Math.floor(totalMinutes / 1440),
        hours: Math.floor((totalMinutes % 1440) / 60),
        minutes: totalMinutes % 60,
    };
}

function DigitBox({ digit }: { digit: string }) {
    return (
        <div
            style={{
                background: "var(--bg-emphasized)",
                borderRadius: "var(--radius-sm)",
                paddingLeft: "0.375rem",
                paddingRight: "0.375rem",
                paddingTop: "0.125rem",
                paddingBottom: "0.125rem",
                minWidth: "1.75rem",
                textAlign: "center",
                fontSize: "1.125rem",
                fontWeight: "bold",
                fontFamily: "monospace",
            }}
        >
            {digit}
        </div>
    );
}

function DigitPair({ value, label }: { value: number; label: string }) {
    const padded = String(Math.min(value, 99)).padStart(2, "0");
    return (
        <VStack gap={0.5} hAlign="center">
            <HStack gap={0.5}>
                <DigitBox digit={padded[0]} />
                <DigitBox digit={padded[1]} />
            </HStack>
            <Text style={{fontSize: "0.75rem", letterSpacing: "0.05em", textShadow: "0 1px 4px rgba(0,0,0,0.85)"}}>
                {label}
            </Text>
        </VStack>
    );
}

type Props = {
    targetUtc: string | null | undefined;
    started: boolean;
};

export function EventCountdown({ targetUtc, started }: Props) {
    if (started || !targetUtc) {
        return (
            <div
                style={{
                    background: "var(--color-success, #22c55e)",
                    color: "white",
                    borderRadius: "var(--radius-sm)",
                    paddingLeft: "0.75rem",
                    paddingRight: "0.75rem",
                    paddingTop: "0.25rem",
                    paddingBottom: "0.25rem",
                    textAlign: "center",
                    fontSize: "0.875rem",
                    fontWeight: "bold",
                }}
            >
                Started
            </div>
        );
    }

    const { days, hours, minutes } = computeRemaining(targetUtc);

    return (
        <HStack gap={3} hAlign="center">
            <DigitPair value={days} label="DAYS" />
            <Text
                style={{fontSize: "1.25rem", fontFamily: "monospace", marginBottom: "1rem", textShadow: "0 1px 4px rgba(0,0,0,0.85)"}}
            >
                :
            </Text>
            <DigitPair value={hours} label="HOURS" />
            <Text
                style={{fontSize: "1.25rem", fontFamily: "monospace", marginBottom: "1rem", textShadow: "0 1px 4px rgba(0,0,0,0.85)"}}
            >
                :
            </Text>
            <DigitPair value={minutes} label="MINUTES" />
        </HStack>
    );
}
