import { Text } from "@astryxdesign/core/Text";
import type { SteamPricingData } from "@src/gen/catalogApi";

type Props = {
    pricing: SteamPricingData | null | undefined;
};

export function PricingCell({ pricing }: Props) {
    if (!pricing || pricing.finalCents == null) {
        return (
            <Text color="secondary">
                -
            </Text>
        );
    }

    if (pricing.finalCents === 0) {
        return (
            <Text color="secondary">
                Free
            </Text>
        );
    }

    if (pricing.initialCents != null && pricing.initialCents > pricing.finalCents) {
        return (
            <Text as="span">
                <Text as="span" color="secondary" style={{textDecoration: "line-through"}}>
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
