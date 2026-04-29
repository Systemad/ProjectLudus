import { defineConfig } from "@kubb/core";
import { pluginOas } from "@kubb/plugin-oas";
import { pluginTs } from "@kubb/plugin-ts";
import { pluginReactQuery } from "@kubb/plugin-react-query";
import { pluginZod } from "@kubb/plugin-zod";
import { QueryKey } from "@kubb/plugin-react-query/components";
import { pluginClient } from "@kubb/plugin-client";

const schemas = [
    { name: "catalogApi", path: "http://localhost:5141/openapi/v1.json" },
    { name: "playApi", path: "http://localhost:5129/openapi/v1.json" },
];

export default defineConfig(() => {
    return schemas.map(({ name, path }) => ({
        name,
        root: ".",
        input: { path },
        output: {
            path: `./src/gen/${name}`,
            clean: true,
            barrelType: "named",
            format: "vp format", //"oxfmt",
            lint: "vp lint", //"oxlint",
        },
        plugins: [
            pluginOas({ collisionDetection: true }),
            pluginTs({
                output: {
                    path: "./types",
                },
                group: {
                    type: "tag",
                    name: ({ group }) => `${group}Types`,
                },
            }),

            pluginReactQuery({
                transformers: {
                    name: (name, type) => {
                        if (type === "file" || type === "function") {
                            return `${name}Hook`;
                        }
                        return name;
                    },
                },
                output: {
                    path: "hooks",
                },
                group: {
                    type: "path",
                },
                queryKey(props) {
                    const keys = QueryKey.getTransformer(props);
                    return ['"v1"', ...keys];
                },
                client: {
                    dataReturnType: "data",
                    baseURL: "",
                },
                mutation: {
                    methods: ["post", "put", "delete"],
                },

                paramsType: "object",
                pathParamsType: "object",
                suspense: {},
                query: {
                    methods: ["get"],
                    importPath: "@tanstack/react-query",
                },
            }),
            pluginZod({
                output: {
                    path: "zod",
                },
            }),
            /*
            pluginClient({
                output: {
                    path: "./clients/axios",
                    barrelType: "named",
                    footer: "",
                },
                group: {
                    type: "tag",
                    name: ({ group }) => `${group}Service`,
                },
                transformers: {
                    name: (name, type) => {
                        return `${name}Client`;
                    },
                },
                operations: true,
                parser: "client",
                include: [
                    {
                        type: "tag",
                        pattern: "Cookie",
                    },
                ],
                pathParamsType: "object",
                dataReturnType: "full",
                client: "axios",
            }),
            */
        ],
    }));
});

/*
        pluginClient({
            output: {
                path: "clients",
            },
        }),
*/
