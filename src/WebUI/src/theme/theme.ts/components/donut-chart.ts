import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { PieChart } from "./pie-chart";

export const DonutChart: ComponentMultiStyle<"DonutChart"> = mergeMultiStyle(
  PieChart,
  {
    baseStyle: {
      label: {},
    },
  },
)();
