import * as React from "react";
import { createLink, type LinkComponent } from "@tanstack/react-router";
import { Link, styled } from "@packages/ui";

interface YamadaLinkProps
    extends Omit<React.ComponentPropsWithoutRef<typeof Link>, "href"> {}

const YamadaLinkComponent = React.forwardRef<
    HTMLAnchorElement,
    YamadaLinkProps
>((props, ref) => {
    return (
        <Link ref={ref} {...props} color="inherit" textDecoration="inherit" />
    );
});

const CreatedLinkComponent = createLink(YamadaLinkComponent);

export const CustomLink: LinkComponent<typeof YamadaLinkComponent> = (
    props
) => {
    return <CreatedLinkComponent preload={"intent"} {...props} />;
};

export const CustomNavigationMenuLinkStyled = styled(CustomLink, {
    base: {
        boxSizing: "border-box",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        gap: "xs",
        height: "10",
        paddingInline: "2",
        margin: 0,
        outline: 0,
        border: "none",
        borderRadius: "lg",
        backgroundColor: "bg.subtle",
        fontFamily: "inherit",
        fontSize: "1rem",
        fontWeight: 500,
        lineHeight: "1.5rem",
        color: "fg.base",
        userSelect: "none",
        textDecoration: "none",
        "@media (max-width: 500px)": {
            fontSize: "0.925rem",
            paddingInline: "2",
        },
        "@media (hover: hover)": {
            "&:hover": {
                backgroundColor: "bg.muted",
            },
        },

        "&[data-popup-open]": {
            backgroundColor: "bg.muted",
        },

        "&:focus-visible": {
            position: "relative",
            outline: "2px solid var(--ui-colors-border-emphasized)",
            outlineOffset: "-1px",
        },
    },
});
