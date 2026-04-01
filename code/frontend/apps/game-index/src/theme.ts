import { defineConfig, defineSemanticTokens, defineStyles, extendTheme, type UsageTheme } from "ui";

export const config = defineConfig({
    css: { varPrefix: "ui" },
    breakpoint: { direction: "up", identifier: "@media screen" },
    defaultColorMode: "dark",
    defaultThemeScheme: "base",
    notice: { duration: 5000 },
    theme: { responsive: true },
});

const colors = defineSemanticTokens.colors({
    bg: {
        base: ["gray.950", "gray.950"],
        panel: ["gray.900", "gray.900"],
        float: ["transparentize(gray.900, 42%)", "transparentize(gray.900, 42%)"],
        muted: ["gray.900", "gray.900"],
        subtle: ["gray.800", "gray.800"],
        emphasized: ["gray.700", "gray.700"],
    },
    fg: {
        base: ["gray.50", "gray.50"],
        contrast: ["#2d2100", "#2d2100"],
        muted: ["gray.300", "gray.300"],
        subtle: ["gray.400", "gray.400"],
        emphasized: ["white", "white"],
    },
    border: {
        base: ["whiteAlpha.200", "whiteAlpha.200"],
        subtle: ["whiteAlpha.100", "whiteAlpha.100"],
        emphasized: ["whiteAlpha.300", "whiteAlpha.300"],
        contrast: ["yellow.500", "yellow.400"],
    },
});

const colorSchemes = defineSemanticTokens.colorSchemes({
    danger: "red",
    error: "red",
    info: "blue",
    link: "yellow",
    mono: ["black", "white"],
    primary: "yellow",
    secondary: "gray",
    success: "green",
    warning: "orange",
});

const layerStyles = defineStyles.layerStyle({
    translucentCard: {
        bg: "transparentize(gray.900, 36%)",
        backdropFilter: "blur(20px) saturate(120%)",
        boxShadow:
            "inset 0 1px 0 {colors.whiteAlpha.200}, inset 0 0 0 1px {colors.whiteAlpha.150}, 0 1px 0 {colors.whiteAlpha.50}, 0 20px 48px -34px {colors.blackAlpha.900}",
    },
    translucentPanel: {
        bg: "transparentize(gray.900, 32%)",
        backdropFilter: "blur(16px) saturate(115%)",
        boxShadow:
            "inset 0 1px 0 {colors.whiteAlpha.150}, inset 0 0 0 1px {colors.whiteAlpha.100}, 0 1px 0 {colors.whiteAlpha.50}, 0 16px 38px -30px {colors.blackAlpha.900}",
    },
    opaqueCard: {
        bg: "bg.panel",
        boxShadow: "inset 0 0 0 1px {colors.whiteAlpha.100}",
    },
});

const globalStyle = defineStyles.globalStyle({
    html: {
        minHeight: "100%",
    },
    body: {
        colorScheme: "gray",
        bg: "bg.base",
        backgroundImage:
            "radial-gradient(1400px 680px at 8% -18%, {colors.whiteAlpha.100} 0%, transparent 64%), radial-gradient(1200px 620px at 92% -10%, {colors.whiteAlpha.50} 0%, transparent 60%), linear-gradient(180deg, {colors.whiteAlpha.50} 0%, transparent 22%)",
        backgroundAttachment: "fixed",
        backgroundRepeat: "no-repeat",
        color: "fg.base",
        fontFamily: "body",
        lineHeight: "moderate",
        margin: "0",
        minHeight: "100%",
        overflowX: "hidden",
        transitionDuration: "moderate",
        transitionProperty: "background-color, color, border-color",
    },
});

export const theme = extendTheme({
    fonts: {
        body: '"Manrope", -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
        heading: '"Space Grotesk", -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
        mono: '"Manrope", ui-monospace, SFMono-Regular, monospace',
    },
    semanticTokens: {
        colorSchemes,
        colors,
    },
    styles: {
        globalStyle,
        layerStyles,
    },
}) as UsageTheme;
