import * as React from "react";
import { createLink, type LinkComponent } from "@tanstack/react-router";
import { Link } from "@yamada-ui/react";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface YamadaLinkProps
    extends Omit<React.ComponentPropsWithoutRef<typeof Link>, "href"> {
    // Add any additional props you want to pass to the link
}

const YamadaLinkComponent = React.forwardRef<
    HTMLAnchorElement,
    YamadaLinkProps
>((props, ref) => {
    return (
        <Link
            ref={ref}
            variant={{
                base: "ghost",
                // Remove default blue color by setting color explicitly
                color: "inherit",
                _hover: { color: "gray.700" },
                _current: { bg: "red", color: "white" },
            }}
            {...props}
        />
    );
});

const CreatedLinkComponent = createLink(YamadaLinkComponent);

export const CustomYamadaLink: LinkComponent<typeof YamadaLinkComponent> = (
    props
) => {
    return (
        <CreatedLinkComponent
            activeOptions={{ exact: true }}
            activeProps={{ style: { color: "red" } }}
            preload={"intent"}
            {...props}
        />
    );
};
