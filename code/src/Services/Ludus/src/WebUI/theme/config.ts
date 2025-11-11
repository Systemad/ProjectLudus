import { extendConfig, type ThemeConfig } from "@yamada-ui/react";

const defaultConfig: ThemeConfig = {
    breakpoint: { direction: "up" /*, identifier: "@media screen"*/ },
    initialColorMode: "dark",
    //initialThemeScheme: "base",
    //var: { prefix: "ui" },
};

export const config = extendConfig(defaultConfig);
