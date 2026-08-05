import { defineConfig } from "vite";
import react, { reactCompilerPreset } from "@vitejs/plugin-react";
import babel from "@rolldown/plugin-babel";
import { tanstackRouter } from "@tanstack/router-plugin/vite";
import path from "path";

// https://vite.dev/config/
// http://localhost:5141
//const target = "http://localhost:53489";
// services__myservice__https__0
const YARP_TARGET = process.env.services__gameIndexApi__https__0 || "http://localhost:5141";

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
        babel({
            plugins: [
                [
                    "@stylexjs/babel-plugin",
                    {
                        dev: true,
                        runtimeInjection: false,
                        genConditionalClasses: true,
                        treeshakeCompensation: true,
                        unstable_moduleResolution: {
                            type: "commonJS",
                            rootDir: __dirname,
                        },
                    },
                ],
            ],
            presets: [reactCompilerPreset()],
        }),
    ],
    server: {
        open: false,
        proxy: {
            //"/api": { target: YARP_TARGET, changeOrigin: true },
            "/catalog": { target: YARP_TARGET, changeOrigin: true },
            //"/play": { target: YARP_TARGET, changeOrigin: true },
        },
    },
});
/*
allowedHosts: ['host.docker.internal']
*/
