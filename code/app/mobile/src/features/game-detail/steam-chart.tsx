import { defineChart, lineY } from "@tanstack/charts/universal";
import { Chart } from "@tanstack/react-native-charts";
import { tooltip } from "@tanstack/react-native-charts/tooltip";
import { scaleLinear } from "@tanstack/charts-scales/linear";
import { useState } from "react";
import { Text as NativeText, View, StyleSheet } from "react-native";
import { Host } from "@expo/ui";
import { SegmentedButton, SingleChoiceSegmentedButtonRow, Text } from "@expo/ui/jetpack-compose";
import { fillMaxWidth, weight } from "@expo/ui/jetpack-compose/modifiers";

import { useSteamGetConcurrentUsersChart } from "@/gen/hooks/SteamHooks/index";
import type { ChartPointDto } from "@/gen/types/ChartPointDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { commonStyles } from "@/shared/ui/common-styles";
import { formatPlayerCount, formatTimestamp, type SteamChartRange } from "@/utils/steam-chart";

const RANGES: readonly { label: string; value: SteamChartRange }[] = [
  { label: "24H", value: "24h" },
  { label: "7D", value: "7d" },
  { label: "30D", value: "30d" },
];

const EMPTY_POINTS: readonly ChartPointDto[] = [];

type ChartDatum = {
  timestamp: number;
  peakPlayers: number;
  avgPlayers: number;
};

function hasPlayerHistory(points: readonly ChartPointDto[]) {
  return points.some((point) => point.peakPlayers > 0 || point.avgPlayers > 0);
}

function toChartData(points: readonly ChartPointDto[]): ChartDatum[] {
  return points
    .map((point) => ({
      timestamp: Date.parse(point.timestamp),
      peakPlayers: point.peakPlayers,
      avgPlayers: point.avgPlayers,
    }))
    .filter((point) => Number.isFinite(point.timestamp));
}

function RangeButton({
  label,
  value,
  selected,
  onSelect,
}: {
  label: string;
  value: SteamChartRange;
  selected: SteamChartRange;
  onSelect: (value: SteamChartRange) => void;
}) {
  return (
    <SegmentedButton
      selected={value === selected}
      onClick={() => onSelect(value)}
      modifiers={[weight(1)]}
    >
      <SegmentedButton.Label>
        <Text>{label}</Text>
      </SegmentedButton.Label>
    </SegmentedButton>
  );
}

function RangeControl({
  selected,
  onSelect,
}: {
  selected: SteamChartRange;
  onSelect: (value: SteamChartRange) => void;
}) {
  const colors = useAppTheme();

  return (
    <Host matchContents={{ vertical: true }} seedColor={colors.primary} style={{ width: "100%" }}>
      <SingleChoiceSegmentedButtonRow modifiers={[fillMaxWidth()]}>
        {RANGES.map((range) => (
          <RangeButton
            key={range.value}
            label={range.label}
            value={range.value}
            selected={selected}
            onSelect={onSelect}
          />
        ))}
      </SingleChoiceSegmentedButtonRow>
    </Host>
  );
}

export function SteamChart({ gameId }: { gameId: string }) {
  const colors = useAppTheme();
  const [range, setRange] = useState<SteamChartRange>("7d");

  const chart24hQuery = useSteamGetConcurrentUsersChart(
    { path: { gameId }, query: { range: "24h" } },
    { query: { retry: false } },
  );
  const chart7dQuery = useSteamGetConcurrentUsersChart(
    { path: { gameId }, query: { range: "7d" } },
    { query: { retry: false } },
  );
  const chart30dQuery = useSteamGetConcurrentUsersChart(
    { path: { gameId }, query: { range: "30d" } },
    { query: { retry: false } },
  );

  const selectedQuery = { "24h": chart24hQuery, "7d": chart7dQuery, "30d": chart30dQuery }[range];
  const hasSteamEntry = chart30dQuery.data ? hasPlayerHistory(chart30dQuery.data.points) : false;
  const selectedPoints = selectedQuery.data?.points ?? EMPTY_POINTS;
  const chartData = toChartData(selectedPoints);
  const definition = defineChart({
    marks: [
      lineY(chartData, {
        id: "peak-players",
        x: "timestamp",
        y: "peakPlayers",
        stroke: colors.primary,
        strokeWidth: 3,
      }),
      lineY(chartData, {
        id: "average-players",
        x: "timestamp",
        y: "avgPlayers",
        stroke: colors.textMuted,
        strokeWidth: 2,
      }),
    ],
    x: {
      scale: scaleLinear,
      axis: {
        ticks: {
          count: 4,
          format: (value) => formatTimestamp(value, range),
        },
      },
    },
    y: {
      scale: scaleLinear,
      nice: true,
      grid: true,
      axis: {
        ticks: {
          count: 4,
          format: formatPlayerCount,
        },
      },
    },
    tooltip: {
      use: tooltip,
      format: (point) =>
        `${formatTimestamp(Number(point.xValue), range)}\n${
          point.groupLabel ?? "Players"
        }: ${formatPlayerCount(Number(point.yValue))}`,
    },
  });

  if (chart30dQuery.isLoading || chart30dQuery.isError || !hasSteamEntry) {
    return null;
  }

  const selectedStatus = getContentStateStatus(
    selectedQuery.isLoading,
    selectedQuery.isError,
    selectedPoints.length === 0 || !hasPlayerHistory(selectedPoints),
  );

  return (
    <View style={commonStyles.section}>
      <NativeText style={[commonStyles.sectionTitle, { color: colors.text }]}>
        Steam players
      </NativeText>
      <RangeControl selected={range} onSelect={setRange} />
      <View
        style={[commonStyles.surfaceCard, styles.card, { backgroundColor: colors.surfaceHigh }]}
      >
        <View style={styles.legend}>
          <View style={styles.legendItem}>
            <View style={[styles.legendDot, { backgroundColor: colors.primary }]} />
            <NativeText style={[styles.legendLabel, { color: colors.textMuted }]}>Peak</NativeText>
          </View>
          <View style={styles.legendItem}>
            <View style={[styles.legendDot, { backgroundColor: colors.textMuted }]} />
            <NativeText style={[styles.legendLabel, { color: colors.textMuted }]}>
              Average
            </NativeText>
          </View>
        </View>
        <ContentState
          status={selectedStatus}
          minHeight={220}
          loading={{ label: "Loading Steam history…" }}
          error={{
            message: "Steam history could not be loaded.",
            onRetry: () => void selectedQuery.refetch(),
          }}
          empty={{
            title: "No player history",
            message: "There is no Steam history for this range yet.",
          }}
        >
          <Chart
            definition={definition}
            accessibilityLabel={`Steam player history for the last ${range}`}
            aspectRatio={1.7}
            color={colors.text}
            focusFill={colors.surfaceHigh}
            style={styles.chart}
          />
        </ContentState>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    padding: 16,
    gap: 12,
  },
  chart: {
    width: "100%",
  },
  legend: {
    flexDirection: "row",
    gap: 16,
  },
  legendItem: {
    alignItems: "center",
    flexDirection: "row",
    gap: 6,
  },
  legendDot: {
    borderRadius: 4,
    height: 8,
    width: 8,
  },
  legendLabel: {
    fontSize: 12,
    fontWeight: "700",
  },
});
