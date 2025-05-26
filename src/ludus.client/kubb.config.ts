import { defineConfig } from "@kubb/core";
import { pluginOas } from "@kubb/plugin-oas";
import { pluginReactQuery } from "@kubb/plugin-react-query";
import { pluginTs } from "@kubb/plugin-ts";
export default defineConfig(() => {
    return {
        root: ".",
        input: {
            path: "../Ludus.Server/Schema/Ludus.Server.json",
        },
        output: {
            path: "./src/gen",
        },
        plugins: [
            pluginOas(),
            pluginTs(),
            pluginReactQuery({
                output: {
                    path: "./hooks",
                },
                group: {
                    type: "tag",
                    name: ({ group }) => `${group}Hooks`,
                },
                client: {
                    dataReturnType: "full",
                    baseURL: "",
                },
                mutation: {
                    methods: ["post", "put", "delete"],
                },
                infinite: {
                    queryParam: "next_page",
                    initialPageParam: 0,
                    cursorParam: "nextCursor",
                },
                query: {
                    methods: ["get"],
                    importPath: "@tanstack/react-query",
                },
                suspense: {},
            }),
        ],
    };
});
