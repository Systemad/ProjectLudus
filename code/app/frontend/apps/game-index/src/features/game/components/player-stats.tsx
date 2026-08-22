"use client";

import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { SegmentedControl, SegmentedControlItem } from "@astryxdesign/core/SegmentedControl";
import { Text } from "@astryxdesign/core/Text";
import { VStack } from "@astryxdesign/core/VStack";
import { useTheme } from "@astryxdesign/core/theme";

import { steamGetConcurrentUsersChartSuspenseQueryOptionsHook } from "@src/gen/catalogApi";

type Props = {
    gameId: number;
};

function SimpleLineChart({ data }: { data: { bucket: string; players: number }[] }) {
    const { token } = useTheme();
    if (!data || data.length === 0) {
        return <Text color="secondary">No chart data available.</Text>;
    }

    const width = 600;
    const height = 200;
    const padding = { top: 20, right: 20, bottom: 30, left: 50 };
    const chartWidth = width - padding.left - padding.right;
    const chartHeight = height - padding.top - padding.bottom;

    const maxPlayers = Math.max(...data.map((d) => d.players));
    const minPlayers = Math.min(...data.map((d) => d.players));
    const range = maxPlayers - minPlayers || 1;

    const points = data.map((d, i) => {
        const x = padding.left + (i / Math.max(data.length - 1, 1)) * chartWidth;
        const y = padding.top + chartHeight - ((d.players - minPlayers) / range) * chartHeight;
        return { x, y, ...d };
    });

    const pathD = points.map((p, i) => `${i === 0 ? "M" : "L"}${p.x},${p.y}`).join(" ");

    return (
        <svg
            width="100%"
            height={height}
            viewBox={`0 0 ${width} ${height}`}
            style={{ overflow: "visible" }}
        >
            {/* Grid lines */}
            {[0, 0.25, 0.5, 0.75, 1].map((ratio) => {
                const y = padding.top + chartHeight - ratio * chartHeight;
                const value = minPlayers + ratio * range;
                return (
                    <g key={ratio}>
                        <line
                            x1={padding.left}
                            y1={y}
                            x2={width - padding.right}
                            y2={y}
                            stroke={token("--color-border")}
                            strokeWidth="1"
                        />
                        <text
                            x={padding.left - 8}
                            y={y + 4}
                            textAnchor="end"
                            fontSize="10"
                            fill={token("--color-text-secondary")}
                        >
                            {Math.round(value)}
                        </text>
                    </g>
                );
            })}
            {/* Line */}
            <path
                d={pathD}
                fill="none"
                stroke={token("--color-data-categorical-blue")}
                strokeWidth="2"
            />
            {/* Dots */}
            {points.map((p, i) => (
                <circle
                    key={i}
                    cx={p.x}
                    cy={p.y}
                    r="3"
                    fill={token("--color-data-categorical-blue")}
                />
            ))}
        </svg>
    );
}

export function PlayerStats({ gameId }: Props) {
    const [range, setRange] = useState<string>("7d");

    const { data } = useQuery({
        ...steamGetConcurrentUsersChartSuspenseQueryOptionsHook({
            path: { gameId: String(gameId) },
            query: { range },
        }),
        placeholderData: (prev: unknown) => prev as any,
    });

    const chartData = (data?.points ?? []).map((p: { timestamp: string; players: number }) => ({
        bucket: p.timestamp,
        players: p.players,
    }));

    return (
        <VStack hAlign="stretch" gap={4} width="100%">
            <Text style={{ fontSize: "1.125rem", fontWeight: 600 }}>Player Activity</Text>

            <SegmentedControl
                value={range}
                onChange={(v) => setRange(v)}
                label="Chart type"
                size="sm"
            >
                <SegmentedControlItem value="48h" label="48 hours" />
                <SegmentedControlItem value="7d" label="7 days" />
                <SegmentedControlItem value="30d" label="30 days" />
            </SegmentedControl>

            <SimpleLineChart data={chartData} />
        </VStack>
    );
}
