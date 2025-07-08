import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { LineChart } from "./line-chart";

export const AreaChart: ComponentMultiStyle<"AreaChart"> = mergeMultiStyle(
  LineChart,
  {
    baseStyle: {
      area: {},
    },
  },
)({ omit: ["line"] });
