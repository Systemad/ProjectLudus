import { Card } from "@astryxdesign/core/Card";
import type { CardProps } from "@astryxdesign/core/Card";

/**
 * A neutral elevated surface for structured, data-heavy content.
 *
 * The gray variant resolves through the active XDS theme's neutral token,
 * providing tonal separation without introducing app-specific colors or shadows.
 */
export function DataCard(props: Omit<CardProps, "variant">) {
    return <Card {...props} variant="gray" />;
}
