import type { LinkComponent } from "@tanstack/react-router";
import { createLink } from "@tanstack/react-router";
import { Button, IconButton, Link } from "ui";

const CreatedLink = createLink(Link);

export const RouterLink: LinkComponent<typeof CreatedLink> = (props) => {
    return <CreatedLink {...props} />;
};

const CreatedLinkButton = createLink(Button);

export const RouterLinkButton: LinkComponent<typeof CreatedLinkButton> = (props) => {
    return <CreatedLinkButton {...props} />;
};

const CreatedLinkIconButton = createLink(IconButton);

export const RouterLinkIconButton: LinkComponent<typeof CreatedLinkIconButton> = (props) => {
    return <CreatedLinkIconButton {...props} />;
};
