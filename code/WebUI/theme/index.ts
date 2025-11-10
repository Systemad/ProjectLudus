import { extendTheme, type UsageTheme } from "@yamada-ui/react";
import { components } from "./components";
import { semantics } from "./semantics";
import { globalStyle, layerStyles, resetStyle, textStyles } from "./styles";
import { tokens } from "./tokens";
/*
export const defaultTheme: UsageTheme = {
    components,
    semantics,
    styles: { globalStyle, layerStyles, resetStyle, textStyles },
    ...tokens,
};
*/

export const defaultTheme: UsageTheme = {
    components,
    semantics,
    styles: { globalStyle, layerStyles, resetStyle, textStyles },
    ...tokens,
};

export const theme = extendTheme(defaultTheme)();

export { config } from "./config";
