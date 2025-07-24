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
    return <Link ref={ref} {...props} />;
});

const CreatedLinkComponent = createLink(YamadaLinkComponent);

export const CustomYamadaLink: LinkComponent<typeof YamadaLinkComponent> = (
    props
) => {
    return <CreatedLinkComponent preload={"intent"} {...props} />;
};

/*
        <CreatedLinkComponent
            textDecoration={"underline"}
            _hover={{ textDecoration: "none" }}
            _focus={{ textDecoration: "none" }}
            preload={"intent"}
            {...props}
*/
