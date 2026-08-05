"use client";

import { useMemo } from "react";
import { Text } from "@astryxdesign/core/Text";
import { Badge } from "@astryxdesign/core/Badge";
import { VStack } from "@astryxdesign/core/VStack";
import { Table } from "@astryxdesign/core/Table";
import { EmptyState } from "@astryxdesign/core/EmptyState";
import { Spinner } from "@astryxdesign/core/Spinner";
import { FaSteam } from "react-icons/fa";
import { useSteamGetPricingHook } from "@src/gen/catalogApi";

type PricingRow = {
    currency: string;
    currentCents: number | null;
    discountPercent: number | null;
    lowCents: number | null;
    highCents: number | null;
};

type Props = {
    gameId: number;
};

function formatCurrency(cents: number, currency: string): string {
    return new Intl.NumberFormat("de-DE", {
        style: "currency",
        currency: currency,
    }).format(cents / 100);
}

export function PricingInfo({ gameId }: Props) {
    const { data: pricing, error, isPending } = useSteamGetPricingHook({ gameId });

    const status = getHttpStatus(error);

    const columns = useMemo(
        () => [
            {
                key: "currency",
                header: "Currency",
                renderCell: (row: PricingRow) => <Text>{row.currency ?? "-"}</Text>,
            },
            {
                key: "currentPrice",
                header: "Current Price",
                renderCell: (row: PricingRow) => {
                    const cents = row.currentCents;
                    const discount = row.discountPercent;
                    if (cents == null) return <Text color="secondary">-</Text>;
                    return (
                        <Text style={{ display: "inline-flex", alignItems: "center", gap: "0.5rem" }}>
                            {formatCurrency(cents, row.currency)}
                            {discount != null && discount > 0 && (
                                <Badge variant="error" label={`-${discount}%`} />
                            )}
                        </Text>
                    );
                },
            },
            {
                key: "lowestPrice",
                header: "Lowest Price",
                renderCell: (row: PricingRow) => {
                    const cents = row.lowCents;
                    if (cents == null) return <Text color="secondary">-</Text>;
                    return (
                        <Text style={{ display: "inline-flex", alignItems: "center", gap: "0.5rem" }}>
                            {formatCurrency(cents, row.currency)}
                            {row.currentCents != null && row.currentCents < cents && (
                                <Badge variant="error" label={`-${Math.round((1 - row.currentCents / cents) * 100)}%`} />
                            )}
                        </Text>
                    );
                },
            },
            {
                key: "highestPrice",
                header: "Highest Price",
                renderCell: (row: PricingRow) => {
                    const cents = row.highCents;
                    if (cents == null) return <Text color="secondary">-</Text>;
                    return formatCurrency(cents, row.currency);
                },
            },
        ],
        [],
    );

    const data = useMemo<PricingRow[]>(
        () => [
            {
                currency: pricing?.currency ?? "EUR",
                currentCents: pricing?.finalCents ?? null,
                discountPercent: pricing?.discountPercent ?? null,
                lowCents: pricing?.low30d ?? null,
                highCents: pricing?.high30d ?? null,
            },
        ],
        [pricing],
    );

    if (isPending) {
        return <Spinner size="lg" />;
    }

    if (status === 404) {
        return (
            <EmptyState
                icon={<FaSteam />}
                title="Steam pricing unavailable"
                description="No Steam pricing data is available for this game yet."
                isCompact
            />
        );
    }

    if (error) {
        return (
            <EmptyState
                icon={<FaSteam />}
                title="Steam pricing could not be loaded"
                description="Please try again shortly."
                isCompact
            />
        );
    }

    return (
        <VStack hAlign="stretch" gap={4}>
            <Text style={{fontSize: "1.125rem", fontWeight: 600}}>
                Pricing
            </Text>
            {pricing?.finalCents == null ? (
                <Text color="secondary">No pricing data available.</Text>
            ) : (
                <Table columns={columns} data={data} />
            )}
        </VStack>
    );
}

function getHttpStatus(error: unknown): number | undefined {
    if (typeof error !== "object" || error === null || !("response" in error)) {
        return undefined;
    }

    const response = error.response;
    if (typeof response !== "object" || response === null || !("status" in response)) {
        return undefined;
    }

    return typeof response.status === "number" ? response.status : undefined;
}
