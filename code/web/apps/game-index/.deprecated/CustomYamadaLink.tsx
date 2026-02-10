import * as React from "react";
import { createLink } from "@tanstack/react-router";
import {
    Button,
    IconButton,
    Link,
    type ButtonProps,
    type IconButtonProps,
    type LinkProps,
} from "@packages/ui";

// --- Link (Usually fine as-is) ---
const YamadaLinkComponent = React.forwardRef<HTMLAnchorElement, LinkProps>(
    (props, ref) => <Link ref={ref} {...props} />
);
export const RouterLink = createLink(YamadaLinkComponent);

// --- Button ---
// We use any on the component ref or cast the ref to fix the HTMLButton vs HTMLAnchor conflict
const YamadaButtonComponent = React.forwardRef<HTMLAnchorElement, ButtonProps>(
    (props, ref) => <Button ref={ref as any} as="a" {...props} />
);
export const RouterLinkButton = createLink(YamadaButtonComponent);

// --- Icon Button ---
const YamadaIconButtonComponent = React.forwardRef<
    HTMLAnchorElement,
    IconButtonProps
>((props, ref) => <IconButton ref={ref as any} as="a" {...props} />);
export const RouterLinkIconButton = createLink(YamadaIconButtonComponent);
