import { defineConfig } from "vite";
import type { Plugin } from "vite";
import react from "@vitejs/plugin-react";
import {
    COLOR_MODE_STORAGE_KEY,
    THEME_SCHEME_STORAGE_KEY,
    getStorageScript,
} from "@packages/ui";
import { tanstackRouter } from "@tanstack/router-plugin/vite";
function injectThemeSchemeScript(): Plugin {
    return {
        name: "inject-theme-scheme-scripts",
        transformIndexHtml(html) {
            const content = getStorageScript(
                "themeScheme",
                THEME_SCHEME_STORAGE_KEY
            )({ defaultValue: "base" });

            return html.replace("<body>", `<body><script>${content}</script>`);
        },
    };
}

function injectColorModeScript(): Plugin {
    return {
        name: "inject-color-mode-script",
        transformIndexHtml(html) {
            const content = getStorageScript(
                "colorMode",
                COLOR_MODE_STORAGE_KEY
            )({ defaultValue: "dark" });

            return html.replace("<body>", `<body><script>${content}</script>`);
        },
    };
}
// https://vite.dev/config/
export default defineConfig({
    plugins: [
        tanstackRouter({
            target: "react",
            autoCodeSplitting: true,
        }),
        react({
            babel: {
                plugins: ["babel-plugin-react-compiler"],
            },
        }),
        injectColorModeScript(),
    ],
});
