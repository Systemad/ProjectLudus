import { Text } from "ui";
import type { SteamPricingData } from "@src/gen/catalogApi";

type Props = {
    pricing: SteamPricingData | null | undefined;
};

export function PricingCell({ pricing }: Props) {
    if (!pricing || pricing.finalCents == null) {
        return (
            <Text color="fgMuted" font="label1">
                —
            </Text>
        );
    }

    if (pricing.finalCents === 0) {
        return (
            <Text color="fgMuted" font="label1">
                Free
            </Text>
        );
    }

    if (pricing.initialCents != null && pricing.initialCents > pricing.finalCents) {
        return (
            <Text as="span" font="body1">
                <Text as="s" color="fgMuted" font="label1">
                    {pricing.initialFormatted ??
                        new Intl.NumberFormat("de-DE", {
                            style: "currency",
                            currency: pricing.currency ?? "EUR",
                        }).format(pricing.initialCents / 100)}
                </Text>{" "}
                {new Intl.NumberFormat("de-DE", {
                    style: "currency",
                    currency: pricing.currency ?? "EUR",
                }).format(pricing.finalCents / 100)}
            </Text>
        );
    }

    return new Intl.NumberFormat("de-DE", {
        style: "currency",
        currency: pricing.currency ?? "EUR",
    }).format(pricing.finalCents / 100) as unknown as JSX.Element;
}
