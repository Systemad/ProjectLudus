import * as React from "react";
import { createLink, type LinkComponent } from "@tanstack/react-router";
import { LinkBox, LinkOverlay } from "@yamada-ui/react";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface YamadaLinkProps
    extends Omit<React.ComponentPropsWithoutRef<typeof LinkOverlay>, "href"> {
    // Add any additional props you want to pass to the link
}

const YamadaLinkComponent = React.forwardRef<
    HTMLAnchorElement,
    YamadaLinkProps
>((props, ref) => {
    return <LinkOverlay ref={ref} {...props} />;
});

const CreatedLinkComponent = createLink(YamadaLinkComponent);

export const CustomLinkOverlay: LinkComponent<typeof YamadaLinkComponent> = (
    props
) => {
    return <CreatedLinkComponent preload={"intent"} {...props} />;
};

//textDecoration={"underline"}
//_hover={{ textDecoration: "none" }}
//_focus={{ textDecoration: "none" }}
