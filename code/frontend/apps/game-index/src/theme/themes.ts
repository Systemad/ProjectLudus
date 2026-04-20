import { defineConfig, defineStyles, extendTheme, type UsageTheme } from "ui";
import { colors } from "./colors";

export const config = defineConfig({
    css: { varPrefix: "ui" },
    breakpoint: { direction: "up", identifier: "@media screen" },
    defaultColorMode: "light",
    defaultThemeScheme: "base",
    notice: { duration: 5000 },
    theme: { responsive: true },
});

const globalStyle = defineStyles.globalStyle({
    html: {
        minHeight: "100%",
    },
    body: {
        colorScheme: "emerald",
        // bg: "bg.base",
        //backgroundImage:
        //    "radial-gradient(1400px 680px at 8% -18%, {colors.whiteAlpha.100} 0%, transparent 64%), radial-gradient(1200px 620px at 92% -10%, {colors.whiteAlpha.50} 0%, transparent 60%), linear-gradient(180deg, {colors.whiteAlpha.50} 0%, transparent 22%)",
        //backgroundAttachment: "fixed",
        //backgroundRepeat: "no-repeat",
        //color: "fg.base",
        fontFamily: "body",
        //lineHeight: "moderate",
        margin: "0",
        minHeight: "100%",
        overflowX: "hidden",
        overflowY: "scroll",
        scrollbarWidth: "thin",
        scrollBehavior: "smooth",
        scrollbarGutter: "stable",
        //transitionDuration: "moderate",
        //transitionProperty: "background-color, color, border-color",
    },
});

export const theme = extendTheme({
    fonts: {
        body: '"Manrope", -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
        heading: '"Space Grotesk", -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
        mono: '"Manrope", ui-monospace, SFMono-Regular, monospace',
    },
    semanticTokens: {
        colors,
    },
    styles: {
        globalStyle,
    },
}) as UsageTheme;
