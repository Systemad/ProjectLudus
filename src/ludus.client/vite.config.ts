import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import fs from "fs";
import path from "path";
import child_process from "child_process";
import { env } from "process";
import { TanStackRouterVite } from "@tanstack/router-plugin/vite";
import tsconfigPaths from "vite-tsconfig-paths";

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ""
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "Ludus.Client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (
        0 !==
        child_process.spawnSync(
            "dotnet",
            [
                "dev-certs",
                "https",
                "--export-path",
                certFilePath,
                "--format",
                "Pem",
                "--no-password",
            ],
            { stdio: "inherit" }
        ).status
    ) {
        throw new Error("Could not create certificate.");
    }
}

//export const target = import.meta.env.VITE_API_BASE_URL;

const target: string = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
      ? env.ASPNETCORE_URLS.split(";")[0]
      : "https://localhost:7247";
// https://vitejs.dev/config/
export default defineConfig({
    plugins: [
        TanStackRouterVite({
            target: "react",
            autoCodeSplitting: true,
            routesDirectory: "./src/routes",
            generatedRouteTree: "./src/routeTree.gen.ts",
            routeFileIgnorePattern: "-",
            quoteStyle: "single",
        }),
        react(),
        tsconfigPaths(),
    ],
    resolve: {
        alias: {
            "@": fileURLToPath(new URL("./src", import.meta.url)),
        },
    },
    server: {
        proxy: {
            "^/WeatherForecast": {
                target,
                secure: false,
            },
            "^/signin": {
                target,
                secure: false,
            },
            "/Auth/steamresponse": {
                target,
                secure: false,
            },
        },
        port: parseInt(env.DEV_SERVER_PORT || "51091"),
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        },
    },
});
