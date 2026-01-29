import { defineConfig } from "@kubb/core";
import { pluginOas } from "@kubb/plugin-oas";
import { pluginReactQuery } from "@kubb/plugin-react-query";
import { QueryKey } from "@kubb/plugin-react-query/components";
import { pluginTs } from "@kubb/plugin-ts";

export default defineConfig(() => {
    return {
        root: ".",
        input: {
            path: "http://localhost:5123/openapi/v1.json",
        },
        output: {
            path: "./src/gen",
            clean: true,
            defaultBanner: "simple",
        },
        plugins: [
            pluginOas({ generators: [], validate: true }),
            pluginTs({
                output: {
                    path: "models",
                    banner(oas) {
                        return `// version: ${oas.api.info.version}`;
                    },
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
                    path: "./hooks",
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
                    importPath: "../../../client.ts",
                    baseURL: "",
                },
                mutation: {
                    methods: ["post", "put", "delete"],
                },
                infinite: {
                    queryParam: "pageNumber",
                    initialPageParam: 1,
                    //cursorParam: "undefined",
                },
                paramsType: "object",
                pathParamsType: "object",
                suspense: {},
                query: {
                    methods: ["get"],
                    importPath: "@tanstack/react-query",
                },
            }),
        ],
    };
});

// https://github.com/kubb-labs/kubb/issues/1632
/*
            hooks: {
                done: ['npm run typecheck', 'biome format --write ./', 'biome lint --fix --unsafe ./src'],
            },
        */

/*
            pluginZod({
                output: {
                    path: "./zod",
                },
                typed: true,
                transformers: {
                    name: (name, type) =>
                        type === "file" ? `${name}.gen` : name,
                },
                //operations: true,
                //importPath: "zod",
                //inferred: true,
            }),
*/
