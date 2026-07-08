import type { LinkComponent } from "@tanstack/react-router";
import { createLink } from "@tanstack/react-router";
import { Link as AstryxLink } from "@astryxdesign/core/Link";
import { Button } from "@astryxdesign/core/Button";
import { IconButton } from "@astryxdesign/core/IconButton";

const CreatedLink = createLink(AstryxLink);

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
