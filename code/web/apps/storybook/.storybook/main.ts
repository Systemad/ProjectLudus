import type { StorybookConfig } from "@storybook/react-vite";
import path, { resolve } from "node:path";
import { mergeConfig } from "vite";

const MONOREPO_ROOT = "../../../";
//const MONOREPO_ROOT = path.resolve(__dirname, "../../");
if (!MONOREPO_ROOT) throw Error("MONOREPO_ROOT is undefined");

const config: StorybookConfig = {
    stories: ["../../../packages/ui/src/**/*.stories.@(js|jsx|mjs|ts|tsx|mdx)"],
    core: {
        builder: "@storybook/builder-vite",
        disableTelemetry: true,
    },
    addons: [
        //       "@storybook/addon-vitest",
        "@storybook/addon-a11y",
        "@storybook/addon-docs",
        "@storybook/addon-onboarding",
    ],
    framework: "@storybook/react-vite",
    typescript: {
        reactDocgen: "react-docgen-typescript",
        reactDocgenTypescriptOptions: {
            // Speeds up Storybook build time
            compilerOptions: {
                allowSyntheticDefaultImports: false,
                esModuleInterop: false,
            },
            // Makes union prop types like variant and size appear as select controls
            shouldExtractLiteralValuesFromEnum: true,
            // Makes string and boolean types that can be undefined appear as inputs and switches
            shouldRemoveUndefinedFromOptional: true,
            // Filter out third-party props from node_modules except @mui packages
            propFilter: (prop) =>
                prop.parent
                    ? !/node_modules\/(?!@mui)/.test(prop.parent.fileName)
                    : true,
        },
    },
    viteFinal: async (config) => {
        return mergeConfig(config, {
            resolve: {
                alias: {
                    "@packages/ui": path.join(
                        import.meta.dirname,
                        "../../../packages/ui/src"
                    ),
                    //"@packages/ui": path.resolve(
                    //    import.meta.dirname,
                    //    "../../packages/ui/src"
                    //),
                    //"@packages/ui": path.resolve(
                    //    MONOREPO_ROOT,
                    //    "packages/ui/src"
                    //),
                },
            },
        });
    },
};
export default config;
