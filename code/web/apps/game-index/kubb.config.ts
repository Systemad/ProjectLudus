import { defineConfig } from "@kubb/core";
import { pluginOas } from "@kubb/plugin-oas";
import { pluginTs } from "@kubb/plugin-ts";
import { pluginClient } from "@kubb/plugin-client";
import { pluginReactQuery } from "@kubb/plugin-react-query";
import { pluginZod } from "@kubb/plugin-zod";
import { QueryKey } from "@kubb/plugin-react-query/components";
export default defineConfig({
    root: ".",
    input: {
        path: "http://localhost:5141/openapi/v1.json",
    },
    output: {
        path: "./src/gen",
        clean: true,
        //defaultBanner: "simple",
    },
    plugins: [
        pluginOas(),
        pluginTs({
            output: {
                path: "models",
                banner(oas) {
                    return `// version: ${oas.api.info.version}`;
                },
            },
        }),
        /*
        pluginClient({
            output: {
                path: "clients",
            },
        }),
        */
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
                //path: "./hooks",
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
                //dataReturnType: "data",
                importPath: "../../../client.ts",
                baseURL: "",
            },
            mutation: {
                methods: ["post", "put", "delete"],
            },
            /*
            infinite: {
                queryParam: "AfterCursor",
                initialPageParam: undefined,
                nextParam: "pageMetadata.nextPageCursor",
                //cursorParam: "data.pageInfo.nextPageCursor",
            },
            */
            paramsType: "object",
            pathParamsType: "object",

            //paramsType: "object",
            //pathParamsType: "object",
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
    ],
});
