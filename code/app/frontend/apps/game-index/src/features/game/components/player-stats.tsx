"use client";

import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { SegmentedControl, Text, LineChart, VStack } from "ui";

import { steamGetConcurrentUsersChartSuspenseQueryOptionsHook } from "@src/gen/catalogApi";

type Props = {
    gameId: number;
};

export function PlayerStats({ gameId }: Props) {
    const [range, setRange] = useState<string>("7d");

    const { data } = useQuery({
        ...steamGetConcurrentUsersChartSuspenseQueryOptionsHook({
            gameId,
            params: { range },
        }),
        placeholderData: (prev: unknown) => prev as any,
    });

    const chartData = (data?.points ?? []).map((p: { timestamp: string; avgPlayers: number }) => ({
        bucket: p.timestamp,
        avgPlayers: p.avgPlayers,
    }));

    const formatter = (label: unknown) => {
        const date = new Date(label as string);
        return range === "30d" || range === "90d" || range === "1y"
            ? date.toLocaleDateString("en-US", { month: "short", day: "numeric" })
            : date.toLocaleTimeString("en-US", { hour: "2-digit", minute: "2-digit" });
    };

    const series = [{ dataKey: "avgPlayers" as const, color: "teal" }];

    return (
        <VStack align="stretch" gap={4} width="100%">
            <Text font="title2" color="fg">
                Player Activity
            </Text>

            <SegmentedControl.Root value={range} onChange={(v) => setRange(v)}>
                <SegmentedControl.Item value="48h">48h</SegmentedControl.Item>
                <SegmentedControl.Item value="7d">7d</SegmentedControl.Item>
                <SegmentedControl.Item value="30d">30d</SegmentedControl.Item>
            </SegmentedControl.Root>

            <LineChart.Root
                data={chartData}
                series={series}
                xAxisProps={{
                    dataKey: "bucket",
                    tickFormatter: (value) => formatter(value),
                }}
                tooltipProps={{ labelFormatter: (value) => formatter(value) }}
            />
        </VStack>
    );
}
