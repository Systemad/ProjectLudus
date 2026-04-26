import { defineConfig } from "ui";

/**
 * The default config of Yamada UI.
 *
 * @see https://yamada-ui.com/docs/theming/configuration/overview
 */
export const config = defineConfig({
    css: { varPrefix: "ui" },
    breakpoint: { direction: "up", identifier: "@media screen" },
    defaultColorMode: "dark",
    defaultThemeScheme: "base",
    notice: { duration: 5000 },
    theme: { responsive: true },
});
