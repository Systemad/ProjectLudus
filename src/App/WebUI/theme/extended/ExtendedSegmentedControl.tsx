import { extendComponentSize, ComponentStyle } from "@yamada-ui/react";

const ExtendedSegmentedControl: ComponentStyle = {
    baseStyle: {
        /**
         * Define a new style
         */
    },
    sizes: extendComponentSize("SegmentedControl", {
        xl: {
            button: { fontSize: "xl", px: "5", py: "2" },
            container: { minW: "lg" },
        },
    }),
};
