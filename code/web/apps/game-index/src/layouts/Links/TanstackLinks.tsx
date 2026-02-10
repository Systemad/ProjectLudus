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
        gap: "0.375rem",
        height: "2.5rem",
        padding: "0 0.5rem",
        margin: 0,
        outline: 0,
        border: "none",
        borderRadius: "0.375rem",
        backgroundColor: "var(--color-gray-50)",
        fontFamily: "inherit",
        fontSize: "1rem",
        fontWeight: 500,
        lineHeight: "1.5rem",
        color: "var(--color-gray-900)",
        userSelect: "none",
        textDecoration: "none",
        "@media (max-width: 500px)": {
            fontSize: "0.925rem",
            padding: "0 0.5rem",
        },
        "@media (hover: hover)": {
            "&:hover": {
                backgroundColor: "var(--color-gray-100)",
            },
        },

        "&[data-popup-open]": {
            backgroundColor: "var(--color-gray-100)",
        },

        "&:focus-visible": {
            position: "relative",
            outline: "2px solid var(--color-blue)",
            outlineOffset: "-1px",
        },
    },
});
