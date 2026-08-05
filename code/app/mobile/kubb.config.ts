import { pluginAxios } from "@kubb/plugin-axios";
import { pluginFetch } from "@kubb/plugin-fetch";
import { pluginReactQuery } from "@kubb/plugin-react-query";
import { pluginRedoc } from "@kubb/plugin-redoc";
import { pluginTs } from "@kubb/plugin-ts";
import { pluginZod } from "@kubb/plugin-zod";
import { defineConfig } from "kubb/config";
import ts from "typescript";

const typePlugin = () =>
  pluginTs({
    printer: {
      nodes: {
        bigint() {
          return ts.factory.createKeywordTypeNode(ts.SyntaxKind.StringKeyword);
        },
      },
    },
  });

export default defineConfig([
  {
    name: "catalog-api",
    root: ".",
    input: "http://localhost:5141/openapi/v1.json",
    output: {
      path: "./src/gen",
      clean: true,
      format: "auto",
      lint: "auto",
    },
    plugins: [
      typePlugin(),
      pluginAxios(),
      pluginZod(),
      pluginRedoc(),
      pluginReactQuery({
        output: { path: "./hooks", mode: "directory" },
        group: {
          type: "tag",
          name: ({ group }) => `${group}Hooks`,
        },
        client: "axios",
        hooks: true,
        infinite: {
          queryParam: "Page",
          initialPageParam: 1,
          nextParam: "nextPage",
        },
        query: {
          methods: ["GET"],
          importPath: "@tanstack/react-query",
        },
        suspense: {},
      }),
    ],
  },
  {
    name: "play-api",
    root: ".",
    input: "http://localhost:5141/openapi/v1.json",
    output: {
      path: "./src/gen/play-api",
      clean: true,
      format: "auto",
      lint: "auto",
    },
    plugins: [
      typePlugin(),
      pluginFetch({ output: { path: "./clients", mode: "directory" } }),
      pluginReactQuery({
        output: { path: "./hooks", mode: "directory" },
        group: {
          type: "tag",
          name: ({ group }) => `${group}Hooks`,
        },
        client: "fetch",
        hooks: true,
        query: {
          methods: ["GET"],
          importPath: "@tanstack/react-query",
        },
      }),
    ],
  },
]);
