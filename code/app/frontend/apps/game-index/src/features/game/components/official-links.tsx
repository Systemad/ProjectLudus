"use client";

import { Grid } from "@astryxdesign/core/Grid";
import { Text } from "@astryxdesign/core/Text";
import { AspectRatio } from "@astryxdesign/core/AspectRatio";
import { Overlay } from "@astryxdesign/core/Overlay";
import { MediaTheme } from "@astryxdesign/core/theme";
import { ClickableCard } from "@astryxdesign/core/ClickableCard";
import * as stylex from "@stylexjs/stylex";
import { LuGlobe } from "react-icons/lu";
import { iconMap } from "@src/icons/platformIconMap";
import type { WebsiteDto } from "@src/gen/catalogApi";
type Props = {
    websites: WebsiteDto[];
};

const styles = stylex.create({
    heading: {
        marginBottom: "var(--spacing-3)",
    },
    empty: {
        color: "var(--color-text-secondary)",
        fontSize: "0.875rem",
    },
    label: {
        fontSize: "1.125rem",
        textAlign: "center",
    },
    media: {
        position: "relative",
        overflow: "hidden",
        backgroundColor: "var(--color-background-surface)",
    },
    brandMark: {
        position: "absolute",
        inset: 0,
        margin: "auto",
        width: "72%",
        height: "72%",
        opacity: 0.36,
        filter: "blur(12px)",
        transform: "scale(1.15)",
    },
});

function normalizeType(type: string | null): string {
    return type ? type.replaceAll(/[-_]/g, " ").replace(/\b\w/g, (letter) => letter.toUpperCase()) : "Official Link";
}

export function OfficialLinks({ websites }: Props) {
    return (
        <div>
            <Text xstyle={styles.heading}>Official Links</Text>
            {websites.length > 0 ? (
                <Grid columns={{minWidth: 280}} gap={3}>
                    {websites.map((website) => (
                        <OfficialLinkCard key={`${website.name ?? "link"}-${website.url ?? ""}`} website={website} />
                    ))}
                </Grid>
            ) : (
                <Text xstyle={styles.empty}>
                    No official links available.
                </Text>
            )}
        </div>
    );
}

function OfficialLinkCard({ website }: { website: WebsiteDto }) {
    const type = website.type?.toLowerCase() ?? "website";
    const BrandIcon = iconMap[type] ?? LuGlobe;
    const label = normalizeType(website.type);

    return (
        <ClickableCard
            label={label}
            href={website.url}
            target="_blank"
            variant="gray"
            padding={0}
        >
            <Overlay
                position="fill"
                align="center"
                scrim={false}
                content={
                    <MediaTheme mode="dark">
                        <Text weight="semibold" xstyle={styles.label}>
                            {label}
                        </Text>
                    </MediaTheme>
                }
            >
                <AspectRatio ratio={16 / 9} xstyle={styles.media}>
                    <div {...stylex.props(styles.brandMark)}>
                        <BrandIcon aria-hidden width="100%" height="100%" />
                    </div>
                </AspectRatio>
            </Overlay>
        </ClickableCard>
    );
}
