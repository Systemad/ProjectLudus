import { defineChart, lineY } from "@tanstack/charts/universal";
import { Chart } from "@tanstack/react-native-charts";
import { tooltip } from "@tanstack/react-native-charts/tooltip";
import { scaleLinear } from "@tanstack/charts-scales/linear";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { Pressable, Text as NativeText, View, StyleSheet } from "react-native";

import { steamGetConcurrentUsersChartQueryOptions } from "@/gen/hooks/SteamHooks/index";
import type { ChartPointDto } from "@/gen/types/ChartPointDto";
import { useAppTheme } from "@/hooks/use-app-theme";
import { ContentState, getContentStateStatus } from "@/shared/ui/content-state";
import { commonStyles } from "@/shared/ui/common-styles";
import {
  formatPlayerCount,
  formatTimestamp,
  formatTooltipTimestamp,
  type SteamChartRange,
} from "@/utils/steam-chart";

const RANGES: readonly { label: string; value: SteamChartRange }[] = [
  { label: "24H", value: "24h" },
  { label: "7D", value: "7d" },
  { label: "30D", value: "30d" },
];

const EMPTY_POINTS: readonly ChartPointDto[] = [];

type ChartDatum = {
  timestamp: number;
  players: number;
};

function hasPlayerHistory(points: readonly ChartPointDto[]) {
  return points.some((point) => point.players > 0);
}

function toChartData(points: readonly ChartPointDto[]): ChartDatum[] {
  return points
    .map((point) => ({
      timestamp: Date.parse(point.timestamp),
      players: point.players,
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
  const colors = useAppTheme();

  return (
    <Pressable
      accessibilityRole="tab"
      accessibilityState={{ selected: value === selected }}
      onPress={() => onSelect(value)}
      style={({ pressed }) => [
        styles.rangeButton,
        {
          backgroundColor: value === selected ? colors.primaryContainer : colors.surface,
          borderColor: colors.outline,
          opacity: pressed ? 0.68 : 1,
        },
      ]}
    >
      <NativeText style={[styles.rangeLabel, { color: colors.text }]}>{label}</NativeText>
    </Pressable>
  );
}

function RangeControl({
  selected,
  onSelect,
}: {
  selected: SteamChartRange;
  onSelect: (value: SteamChartRange) => void;
}) {
  return (
    <View style={styles.rangeRow}>
      {RANGES.map((range) => (
        <RangeButton
          key={range.value}
          label={range.label}
          value={range.value}
          selected={selected}
          onSelect={onSelect}
        />
      ))}
    </View>
  );
}

export function SteamChart({
  gameId,
  onTooltipDismiss,
}: {
  gameId: string;
  onTooltipDismiss: (dismiss: () => void) => void;
}) {
  const colors = useAppTheme();
  const [range, setRange] = useState<SteamChartRange>("24h");

  const chartQuery = useQuery({
    ...steamGetConcurrentUsersChartQueryOptions({
      path: { gameId },
      query: { range },
    }),
    placeholderData: keepPreviousData,
    retry: false,
  });
  const selectedPoints = chartQuery.data?.points ?? EMPTY_POINTS;
  const hasChartData = selectedPoints.length > 0 && hasPlayerHistory(selectedPoints);
  const selectedStatus = getContentStateStatus(
    chartQuery.isLoading && !hasChartData,
    chartQuery.isError && !hasChartData,
    !hasChartData,
  );
  const chartData = toChartData(selectedPoints);
  const definition = defineChart({
    marks: [
      lineY(chartData, {
        id: "players",
        x: "timestamp",
        y: "players",
        stroke: colors.primary,
        strokeWidth: 3,
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
      sticky: false,
      format: (point) => {
        const label = point.groupLabel === "players" ? "Players" : (point.groupLabel ?? "Players");
        return `${formatTooltipTimestamp(Number(point.xValue))}\n${label}: ${formatPlayerCount(
          Number(point.yValue),
        )}`;
      },
    },
  });

  return (
    <View style={styles.section}>
      <View style={styles.sectionHeader}>
        <RangeControl selected={range} onSelect={setRange} />
      </View>
      <View
        style={[commonStyles.surfaceCard, styles.card, { backgroundColor: colors.surfaceHigh }]}
      >
        <View style={styles.chartHeader}>
          <View style={styles.legend}>
            <View style={styles.legendItem}>
              <View style={[styles.legendDot, { backgroundColor: colors.primary }]} />
              <NativeText style={[styles.legendLabel, { color: colors.textMuted }]}>
                Players
              </NativeText>
            </View>
            <NativeText style={[styles.legendNote, { color: colors.textMuted }]}>
              UTC / local
            </NativeText>
          </View>
          <NativeText numberOfLines={1} style={[styles.chartTitle, { color: colors.text }]}>
            Steam players
          </NativeText>
        </View>
        <ContentState
          status={selectedStatus}
          minHeight={220}
          loading={{ label: "Loading Steam history…" }}
          error={{
            message: "Steam history could not be loaded.",
            onRetry: () => void chartQuery.refetch(),
          }}
          empty={{
            title: "No player history",
            message: "There is no Steam history for this range yet.",
          }}
        >
          <Chart
            definition={definition}
            accessibilityLabel={`Player history for the last ${range}`}
            aspectRatio={1.7}
            color={colors.text}
            focusFill={colors.surfaceHigh}
            renderTooltip={({ defaultBody, dismiss }) => {
              onTooltipDismiss(dismiss);
              return defaultBody;
            }}
            style={styles.chart}
          />
        </ContentState>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  section: {
    gap: 12,
  },
  card: {
    paddingHorizontal: 12,
    paddingVertical: 12,
    gap: 12,
  },
  sectionHeader: {
    alignItems: "center",
    flexDirection: "row",
    gap: 8,
  },
  rangeRow: {
    flex: 1,
    flexDirection: "row",
    gap: 4,
    marginLeft: "auto",
  },
  rangeButton: {
    alignItems: "center",
    borderRadius: 18,
    borderWidth: StyleSheet.hairlineWidth,
    flex: 1,
    justifyContent: "center",
    minHeight: 38,
  },
  rangeLabel: {
    fontSize: 11,
    fontWeight: "800",
  },
  chart: {
    width: "100%",
  },
  chartHeader: {
    alignItems: "center",
    flexDirection: "row",
    gap: 12,
    justifyContent: "space-between",
  },
  chartTitle: {
    flexShrink: 1,
    fontSize: 12,
    fontWeight: "800",
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
  legendNote: {
    fontSize: 11,
    fontWeight: "600",
  },
});
