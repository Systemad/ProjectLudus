import { adapterOas } from "@kubb/adapter-oas";
import { pluginAxios } from "@kubb/plugin-axios";
import { pluginReactQuery, resolverReactQuery } from "@kubb/plugin-react-query";
import { pluginTs } from "@kubb/plugin-ts";
import { pluginZod } from "@kubb/plugin-zod";
import { defineConfig } from "kubb";

const schemas = [
    { name: "catalogApi", path: "../../../backend/docs/openapi/Backend.API.json" },
    // { name: "playApi", path: "http://localhost:5129/openapi/v1.json" },
];

const hookResolver = {
    query: {
        name: (node: Parameters<typeof resolverReactQuery.query.name>[0]) => `${resolverReactQuery.query.name(node)}Hook`,
        optionsName: (node: Parameters<typeof resolverReactQuery.query.optionsName>[0]) => `${resolverReactQuery.query.optionsName(node)}Hook`,
        clientName: (node: Parameters<typeof resolverReactQuery.query.clientName>[0]) => `${resolverReactQuery.query.clientName(node)}Hook`,
    },
    suspenseQuery: {
        name: (node: Parameters<typeof resolverReactQuery.suspenseQuery.name>[0]) => `${resolverReactQuery.suspenseQuery.name(node)}Hook`,
        optionsName: (node: Parameters<typeof resolverReactQuery.suspenseQuery.optionsName>[0]) => `${resolverReactQuery.suspenseQuery.optionsName(node)}Hook`,
        clientName: (node: Parameters<typeof resolverReactQuery.suspenseQuery.clientName>[0]) => `${resolverReactQuery.suspenseQuery.clientName(node)}Hook`,
    },
    mutation: {
        name: (node: Parameters<typeof resolverReactQuery.mutation.name>[0]) => `${resolverReactQuery.mutation.name(node)}Hook`,
        optionsName: (node: Parameters<typeof resolverReactQuery.mutation.optionsName>[0]) => `${resolverReactQuery.mutation.optionsName(node)}Hook`,
    },
};

export default defineConfig(() =>
    schemas.map(({ name, path }) => ({
        name,
        root: ".",
        input: path,
        adapter: adapterOas({
            integerType: "number",
            unknownType: "void",
            emptySchemaType: "void",
        }),
        output: {
            path: `./src/gen/${name}`,
            clean: true,
            barrel: { type: "named" },
            format: "vp format",
            lint: "vp lint",
        },
        plugins: [
            pluginTs({
                output: {
                    path: "./types",
                    mode: "directory",
                },
                group: {
                    type: "tag",
                    name: ({ group }) => `${group}Types`,
                },
            }),
            pluginAxios({
                output: {
                    path: "./clients",
                    mode: "directory",
                    barrel: { type: "named" },
                },
            }),
            pluginReactQuery({
                client: "axios",
                hooks: true,
                resolver: hookResolver,
                output: {
                    path: "hooks",
                    mode: "directory",
                },
                group: {
                    type: "path",
                },
                mutation: {
                    methods: ["post", "put", "delete"],
                },
                suspense: {},
                query: {
                    methods: ["get"],
                },
            }),
            pluginZod({
                output: {
                    path: "zod",
                    mode: "directory",
                    barrel: { type: "named" },
                },
            }),
        ],
    })),
);
