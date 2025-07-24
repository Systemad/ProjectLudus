import { components } from "./components";
import { semantics } from "./semantics";
import { globalStyle, layerStyles, resetStyle, textStyles } from "./styles";
import { tokens } from "./tokens";

export const defaultTheme = {
  components,
  semantics,
  styles: { globalStyle, layerStyles, resetStyle, textStyles },
  ...tokens,
};

export { config } from "./config";
