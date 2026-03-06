import * as React from "react";
import { createLink, type LinkComponent } from "@tanstack/react-router";
import {
    Link as YamadaLink,
    type LinkProps as YamadaLinkProps,
    Button as YamadaButton,
    type ButtonProps as YamadaButtonProps,
    IconButton as YamadaIconButton,
    type IconButtonProps as YamadaIconButtonProps,
} from "@packages/ui";

/* --- 1. Base Link --- */
const YamadaLinkComponent = React.forwardRef<
    HTMLAnchorElement,
    YamadaLinkProps
>((props, ref) => <YamadaLink ref={ref} {...props} />);

const CreatedLink = createLink(YamadaLinkComponent);

export const RouterLink: LinkComponent<typeof YamadaLinkComponent> = (
    props
) => <CreatedLink preload="intent" {...props} />;

const YamadaButtonComponent = React.forwardRef<
    HTMLAnchorElement,
    YamadaButtonProps
>((props, ref) => <YamadaButton as="a" ref={ref as any} {...props} />);

const CreatedButtonLink = createLink(YamadaButtonComponent);

export const RouterLinkButton: LinkComponent<typeof YamadaButtonComponent> = (
    props
) => <CreatedButtonLink preload="intent" {...props} />;

const YamadaIconButtonComponent = React.forwardRef<
    HTMLAnchorElement,
    YamadaIconButtonProps
>((props, ref) => <YamadaIconButton as="a" ref={ref as any} {...props} />);

const CreatedIconButtonLink = createLink(YamadaIconButtonComponent);

export const RouterLinkIconButton: LinkComponent<
    typeof YamadaIconButtonComponent
> = (props) => <CreatedIconButtonLink preload="intent" {...props} />;
