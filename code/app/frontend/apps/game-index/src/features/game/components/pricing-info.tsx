"use client";

import { useMemo } from "react";
import { useSuspenseQuery } from "@tanstack/react-query";
import { Text, Tag, VStack, Table } from "ui";
import { steamGetPricingSuspenseQueryOptionsHook } from "@src/gen/catalogApi";

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
    const { data: pricing } = useSuspenseQuery(steamGetPricingSuspenseQueryOptionsHook({ gameId }));

    const columns = useMemo(
        () => [
            {
                header: "Currency",
                accessor: (row: PricingRow) => <Text font="body1">{row.currency ?? "—"}</Text>,
            },
            {
                header: "Current Price",
                accessor: (row: PricingRow) => {
                    const cents = row.currentCents;
                    const discount = row.discountPercent;
                    if (cents == null) return <Text color="fgMuted">—</Text>;
                    return (
                        <Text as="span" font="body1" display="inline-flex" align="center" gap={2}>
                            {formatCurrency(cents, row.currency)}
                            {discount != null && discount > 0 && (
                                <Tag variant="subtle" colorScheme={"red"} size="sm">
                                    -{discount}%
                                </Tag>
                            )}
                        </Text>
                    );
                },
            },
            {
                header: "Lowest Price",
                accessor: (row: PricingRow) => {
                    const cents = row.lowCents;
                    if (cents == null) return <Text color="fgMuted">—</Text>;
                    return (
                        <Text as="span" font="body1" display="inline-flex" align="center" gap={2}>
                            {formatCurrency(cents, row.currency)}
                            {row.currentCents != null && row.currentCents < cents && (
                                <Tag variant="subtle" colorScheme={"red"} size="sm">
                                    -{Math.round((1 - row.currentCents / cents) * 100)}%
                                </Tag>
                            )}
                        </Text>
                    );
                },
            },
            {
                header: "Highest Price",
                accessor: (row: PricingRow) => {
                    const cents = row.highCents;
                    if (cents == null) return <Text color="fgMuted">—</Text>;
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

    return (
        <VStack align="stretch" gap={4}>
            <Text font="title2" color="fg">
                Pricing
            </Text>
            {pricing?.finalCents == null ? (
                <Text color="fgMuted">No pricing data available.</Text>
            ) : (
                <Table columns={columns} data={data} />
            )}
        </VStack>
    );
}
