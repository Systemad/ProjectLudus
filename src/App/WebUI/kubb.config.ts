import { defineConfig } from "@kubb/core";
import { pluginOas } from "@kubb/plugin-oas";
import { pluginReactQuery } from "@kubb/plugin-react-query";
import { QueryKey } from "@kubb/plugin-react-query/components";
import { pluginTs } from "@kubb/plugin-ts";
export default defineConfig(() => {
    return {
        root: ".",
        input: {
            // TODO: Change to correct port
            path: "http://localhost:5123/openapi/v1.json",
        },
        output: {
            path: "./src/gen",
            clean: true,
            defaultBanner: "simple",
        },
        /*
            hooks: {
                done: ['npm run typecheck', 'biome format --write ./', 'biome lint --fix --unsafe ./src'],
            },
        */
        plugins: [
            pluginOas({ generators: [] }),
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
                    baseURL: "",
                },
                mutation: {
                    methods: ["post", "put", "delete"],
                },
                infinite: {
                    queryParam: "pageNumber",
                    initialPageParam: 1,
                    cursorParam: "undefined",
                },
                paramsType: "inline",
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
