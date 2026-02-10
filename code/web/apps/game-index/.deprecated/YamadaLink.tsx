import type {
    ButtonProps,
    HTMLRefAttributes,
    IconButtonProps,
    LinkProps,
    Merge,
} from "@packages/ui";
import type { FC } from "react";
import type { LinkProps as OriginalLinkProps } from "@packages/ui";
import { Button, IconButton, Link } from "@packages/ui";
import { Link as TanstackLink } from "@tanstack/react-router";

export interface RouterLinkProps
    extends Omit<Merge<OriginalLinkProps, LinkProps>, "as" | "ref"> {}

export const RouterLink: FC<RouterLinkProps> = (props) => {
    return <Link as={TanstackLink} {...props} preload={"intent"} />;
};

export interface RouterLinkButtonProps
    extends Omit<Merge<OriginalLinkProps, ButtonProps>, "as" | "ref">,
        HTMLRefAttributes<"a"> {}

export const RouterLinkButton: FC<RouterLinkButtonProps> = (props) => {
    return <Button as={TanstackLink} {...props} preload={"intent"} />;
};

export interface RouterLinkIconButtonProps
    extends Omit<Merge<OriginalLinkProps, IconButtonProps>, "as" | "ref">,
        HTMLRefAttributes<"a"> {}

export const RouterLinkIconButton: FC<RouterLinkIconButtonProps> = (props) => {
    return <IconButton as={TanstackLink} {...props} preload={"intent"} />;
};
