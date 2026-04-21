import { defineConfig } from "vite";
import react, { reactCompilerPreset } from "@vitejs/plugin-react";
import babel from "@rolldown/plugin-babel";
import { tanstackRouter } from "@tanstack/router-plugin/vite";
import path from "path";
import type { Plugin } from "vite";
import { COLOR_MODE_STORAGE_KEY, getStorageScript } from "ui";
// https://vite.dev/config/
// http://localhost:5141
const target = "http://localhost:53489";
//const target = "http://localhost:5141"; //process.env.CATALOGAPI_HTTPS || process.env.CATALOGAPI_HTTP;

function injectColorModeScript(): Plugin {
    return {
        name: "inject-color-mode-script",
        transformIndexHtml(html) {
            const content = getStorageScript(
                "colorMode",
                COLOR_MODE_STORAGE_KEY,
            )({ defaultValue: "dark" });

            return html.replace("<body>", `<body><script>${content}</script>`);
        },
    };
}

export default defineConfig({
    resolve: {
        alias: {
            "@src": path.resolve(__dirname, "src"),
        },
    },
    plugins: [
        tanstackRouter({
            target: "react",
            autoCodeSplitting: true,
        }),
        react(),
        babel({ presets: [reactCompilerPreset()] }),
        injectColorModeScript(),
    ],
    server: {
        open: true,
        proxy: {
            "/api": {
                target,
                changeOrigin: true,
            },
        },
    },
});
/*

*/
