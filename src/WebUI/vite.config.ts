import { fileURLToPath, URL } from "node:url";

import { defineConfig, loadEnv } from "vite";
import type { Plugin } from "vite";
import plugin from "@vitejs/plugin-react";
import { tanstackRouter } from "@tanstack/router-plugin/vite";
import tsconfigPaths from "vite-tsconfig-paths";

import { defaultConfig, getColorModeScript } from "@yamada-ui/react";

function injectScript(): Plugin {
    return {
        name: "vite-plugin-inject-scripts",
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        transformIndexHtml(html, _) {
            const content = getColorModeScript({
                initialColorMode: defaultConfig.initialColorMode,
            });

            return html.replace("<body>", `<body><script>${content}</script>`);
        },
    };
}

//const target = process.env.BACKEND_URL;
//const port = process.env.PORT ? parseInt(process.env.PORT) : undefined;

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const env = loadEnv(mode, process.cwd(), "");
    return {
        plugins: [
            tanstackRouter({
                target: "react",
                autoCodeSplitting: true,
                routesDirectory: "./src/routes",
                generatedRouteTree: "./src/routeTree.gen.ts",
                routeFileIgnorePattern: "-",
                quoteStyle: "single",
            }),
            plugin(),
            tsconfigPaths(),
            injectScript(),
        ],
        resolve: {
            alias: {
                "@": fileURLToPath(new URL("./src", import.meta.url)),
            },
        },
        server: {
            //open: true,
            //port: port,
            proxy: {
                /*
                "/api": {
                    target:
                        process.env.services__apiservice__https__0 ||
                        process.env.services__apiservice__http__0,
                    changeOrigin: true,
                    secure: false,
                    pathRewrite: { "^/api": "" },
                    //rewrite: (path) => path.replace(/^\/api/, ""),
                },
*/
                "/api": {
                    target:
                        process.env.services__apiservice__https__0 ||
                        process.env.services__apiservice__http__0,
                    changeOrigin: true,
                    secure: false,
                    pathRewrite: { "^/api": "" },
                    //rewrite: (path) => path.replace(/^\/api/, ""),
                },
                /*
            "^/weatherforecast": {
                target,
                secure: false,
            },
            "^/api/standings(/|$)": {
                target,
                //secure: false,
                changeOrigin: true,
            },
            "^/api/markdown": {
                target,
                //secure: false,
                changeOrigin: true,
            },
            */
            },
        },
        build: {
            outDir: "dist",
            rollupOptions: {
                input: ".index.html",
            },
        },
    };
});
