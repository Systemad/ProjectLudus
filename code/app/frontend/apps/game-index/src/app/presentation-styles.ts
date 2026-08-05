import * as stylex from "@stylexjs/stylex";

export const presentationStyles = stylex.create({
    shell: {
        display: "flex",
        flexDirection: "column",
        minHeight: "100%",
        width: "100%",
    },
    content: {
        flex: 1,
        maxWidth: 1128,
        marginInline: "auto",
        width: "100%",
    },
    navigationSurface: {
        backgroundColor: "color-mix(in srgb, var(--color-background-surface) 86%, transparent)",
        borderBottom: "1px solid var(--color-border)",
        backdropFilter: "blur(12px)",
    },
    mobileNavigationToggle: {
        display: "none",
        "@media (max-width: 640px)": {
            display: "inline-flex",
        },
    },
    pageHeader: {
        display: "flex",
        alignItems: "end",
        justifyContent: "space-between",
        gap: "var(--spacing-4)",
        paddingBottom: "var(--spacing-4)",
        borderBottom: "1px solid var(--color-border)",
        "@media (max-width: 640px)": {
            alignItems: "start",
            flexDirection: "column",
        },
    },
    metric: {
        fontVariantNumeric: "tabular-nums",
        fontFamily: "ui-monospace, SFMono-Regular, Consolas, monospace",
    },
    loadingCenter: {
        display: "grid",
        minHeight: "16rem",
        placeItems: "center",
    },
});
