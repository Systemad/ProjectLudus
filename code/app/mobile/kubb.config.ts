import { adapterOas } from "@kubb/adapter-oas";
import { pluginAxios } from "@kubb/plugin-axios";
import { pluginFetch } from "@kubb/plugin-fetch";
import { pluginReactQuery } from "@kubb/plugin-react-query";
import { pluginRedoc } from "@kubb/plugin-redoc";
import { pluginTs } from "@kubb/plugin-ts";
import { pluginZod } from "@kubb/plugin-zod";
import { defineConfig } from "kubb/config";
import { ast } from "kubb/kit";

const nullableReferenceUnion = ast.defineMacro({
  name: "nullable-reference-union",
  property(node) {
    if (
      node.schema.type !== "union" ||
      !node.schema.members?.some((member) => member.type === "ref")
    ) {
      return;
    }

    return {
      ...node,
      schema: {
        ...node.schema,
        members: node.schema.members.map((member) =>
          member.type === "void"
            ? ast.factory.createSchema({ type: "null", primitive: "null" })
            : member,
        ),
      },
    };
  },
});

export default defineConfig([
  {
    name: "catalog-api",
    root: ".",
    input: "../backend/docs/openapi/Backend.API.json",
    adapter: adapterOas({
      integerType: "number",
      unknownType: "void",
      emptySchemaType: "void",
    }),
    output: {
      path: "./src/gen",
      clean: true,
      format: false,
      lint: false,
      barrel: { type: "named" },
    },
    plugins: [
      pluginTs({
        macros: [nullableReferenceUnion],
        output: { path: "./types", mode: "directory", barrel: { type: "named" } },
      }),
      pluginAxios({
        output: { path: "./clients", mode: "directory", barrel: { type: "named" } },
      }),
      pluginZod({
        macros: [nullableReferenceUnion],
        output: { path: "./zod", mode: "directory", barrel: { type: "named" } },
      }),
      pluginRedoc(),
      pluginReactQuery({
        output: { path: "./hooks", mode: "directory", barrel: { type: "named" } },
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
    input: "../backend/docs/openapi/Backend.API.json",
    adapter: adapterOas({
      integerType: "number",
      unknownType: "void",
      emptySchemaType: "void",
    }),
    output: {
      path: "./src/gen/play-api",
      clean: true,
      format: false,
      lint: false,
      barrel: { type: "named" },
    },
    plugins: [
      pluginTs({
        macros: [nullableReferenceUnion],
        output: { path: "./types", mode: "directory", barrel: { type: "named" } },
      }),
      pluginFetch({
        output: { path: "./clients", mode: "directory", barrel: { type: "named" } },
      }),
      pluginReactQuery({
        output: { path: "./hooks", mode: "directory", barrel: { type: "named" } },
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
