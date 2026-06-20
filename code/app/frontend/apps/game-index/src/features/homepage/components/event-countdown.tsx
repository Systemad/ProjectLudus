import { Box, HStack, Text, VStack } from "ui";

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
        <Box
            bg="bg.emphasized"
            rounded="sm"
            px="1.5"
            py="0.5"
            minW="7"
            textAlign="center"
            fontSize="lg"
            fontWeight="bold"
            fontFamily="mono"
        >
            {digit}
        </Box>
    );
}

function DigitPair({ value, label }: { value: number; label: string }) {
    const padded = String(Math.min(value, 99)).padStart(2, "0");
    return (
        <VStack gap="0.5" align="center">
            <HStack gap="0.5">
                <DigitBox digit={padded[0]} />
                <DigitBox digit={padded[1]} />
            </HStack>
            <Text fontSize="xs" letterSpacing="wide" textShadow="0 1px 4px rgba(0,0,0,0.85)">
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
            <Box
                bg="success"
                color="white"
                rounded="sm"
                px="3"
                py="1"
                textAlign="center"
                fontSize="sm"
                fontWeight="bold"
            >
                Started
            </Box>
        );
    }

    const { days, hours, minutes } = computeRemaining(targetUtc);

    return (
        <HStack gap="3" justify="center">
            <DigitPair value={days} label="DAYS" />
            <Text
                fontSize="xl"
                fontFamily="mono"
                alignSelf="center"
                mb="4"
                textShadow="0 1px 4px rgba(0,0,0,0.85)"
            >
                :
            </Text>
            <DigitPair value={hours} label="HOURS" />
            <Text
                fontSize="xl"
                fontFamily="mono"
                alignSelf="center"
                mb="4"
                textShadow="0 1px 4px rgba(0,0,0,0.85)"
            >
                :
            </Text>
            <DigitPair value={minutes} label="MINUTES" />
        </HStack>
    );
}
