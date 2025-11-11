import { mergeMultiStyle } from "@yamada-ui/react";
import type { ComponentMultiStyle } from "@yamada-ui/react";
import { Slider } from "./slider";

export const RangeSlider: ComponentMultiStyle<"RangeSlider"> =
  mergeMultiStyle(Slider)();
