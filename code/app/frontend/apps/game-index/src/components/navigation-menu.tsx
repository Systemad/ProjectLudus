import { NavigationMenu as Base } from "@base-ui/react/navigation-menu";
import { styled } from "ui";

export const NavMenuRoot = styled(Base.Root, {
    base: {
        display: "flex",
    },
});

export const NavMenuList = styled(Base.List, {
    base: {
        display: "flex",
        alignItems: "center",
        listStyle: "none",
        p: "0",
        m: "0",
        gap: "2",
    },
});

export const NavMenuItem = styled(Base.Item);

export const NavMenuTrigger = styled(Base.Trigger, {
    base: {
        display: "inline-flex",
        alignItems: "center",
        gap: "1",
        px: "3",
        py: "1.5",
        rounded: "full",
        bg: "transparent",
        color: "fg.base",
        cursor: "pointer",
        textDecoration: "none",
        border: "none",
        fontFamily: "inherit",
        fontSize: "md",
        fontWeight: "bold",
        transitionDuration: "moderate",
        transitionProperty: "common",
        _hover: { bg: "whiteAlpha.100" },
        // @ts-expect-error
        _popupOpen: { bg: "whiteAlpha.200" },
        _focusVisible: {
            outline: "2px solid",
            outlineColor: "blue.500",
            outlineOffset: "-1px",
        },
    },
});

export const NavMenuIcon = styled(Base.Icon, {
    base: {
        transitionDuration: "moderate",
        transitionProperty: "transform",
        "&[data-popup-open]": {
            transform: "rotate(180deg)",
        },
    },
});

export const NavMenuPortal = Base.Portal;

export const NavMenuPositioner = styled(Base.Positioner, {
    base: {
        "&::before": {
            content: "''",
            position: "absolute",
        },
        "&[data-side='top']::before": {
            left: "0",
            right: "0",
            bottom: "-10px",
            h: "2.5",
        },
        "&[data-side='bottom']::before": {
            left: "0",
            right: "0",
            top: "-10px",
            h: "2.5",
        },
        "&[data-side='left']::before": {
            top: "0",
            bottom: "0",
            right: "-10px",
            w: "2.5",
        },
        "&[data-side='right']::before": {
            top: "0",
            bottom: "0",
            left: "-10px",
            w: "2.5",
        },
    },
});

export const NavMenuPopup = styled(Base.Popup, {
    base: {
        position: "relative",
        bg: "bg.panel",
        rounded: "lg",
        boxShadow: "lg",
        borderWidth: "1px",
        borderColor: "border",
        outline: "none",
        transformOrigin: "var(--transform-origin)",
        transitionDuration: "0.35s",
        transitionProperty: "opacity, transform",
        transitionTimingFunction: "cubic-bezier(0.22, 1, 0.36, 1)",
        "&[data-starting-style], &[data-ending-style]": {
            opacity: 0,
            transform: "scale(0.9)",
        },
        "&[data-ending-style]": {
            transitionDuration: "0.15s",
            transitionTimingFunction: "ease",
        },
    },
});

export const NavMenuContent = styled(Base.Content, {
    base: {
        boxSizing: "border-box",
        p: "6",
        minW: { base: "calc(100vw - 40px)", md: "md" },
        transitionDuration: "0.35s",
        transitionProperty: "opacity, transform",
        transitionTimingFunction: "cubic-bezier(0.22, 1, 0.36, 1)",
        "&[data-starting-style], &[data-ending-style]": {
            opacity: 0,
        },
        "&[data-starting-style][data-activation-direction='left']": {
            transform: "translateX(-30%)",
        },
        "&[data-starting-style][data-activation-direction='right']": {
            transform: "translateX(30%)",
        },
        "&[data-ending-style][data-activation-direction='left']": {
            transform: "translateX(30%)",
        },
        "&[data-ending-style][data-activation-direction='right']": {
            transform: "translateX(-30%)",
        },
    },
});

export const NavMenuViewport = styled(Base.Viewport, {
    base: {
        position: "relative",
        overflow: "hidden",
        w: "full",
        h: "full",
    },
});

export const NavMenuArrow = styled(Base.Arrow, {
    base: {
        display: "flex",
        "&[data-side='top']": {
            bottom: "-8px",
            rotate: "180deg",
        },
        "&[data-side='bottom']": {
            top: "-8px",
            rotate: "0deg",
        },
        "&[data-side='left']": {
            right: "-13px",
            rotate: "90deg",
        },
        "&[data-side='right']": {
            left: "-13px",
            rotate: "-90deg",
        },
    },
});

export const NavMenuBackdrop = Base.Backdrop;

export const NavMenuLink = styled(Base.Link, {
    base: {
        display: "inline-flex",
        alignItems: "center",
        px: "3",
        py: "1.5",
        rounded: "full",
        color: "fg.base",
        textDecoration: "none",
        fontWeight: "bold",
        fontSize: "md",
        transitionDuration: "moderate",
        transitionProperty: "common",
        _hover: { bg: "whiteAlpha.100" },
        _focusVisible: {
            outline: "2px solid",
            outlineColor: "blue.500",
            outlineOffset: "-1px",
        },
    },
});

export function ChevronDownIcon(props: React.ComponentProps<"svg">) {
    return (
        <svg width="10" height="10" viewBox="0 0 10 10" fill="none" {...props}>
            <path d="M1 3.5L5 7.5L9 3.5" stroke="currentColor" strokeWidth="1.5" />
        </svg>
    );
}

export function NavArrowSvg(props: React.ComponentProps<"svg">) {
    return (
        <svg width="20" height="10" viewBox="0 0 20 10" fill="none" {...props}>
            <path
                d="M9.66437 2.60207L4.80758 6.97318C4.07308 7.63423 3.11989 8 2.13172 8H0V10H20V8H18.5349C17.5468 8 16.5936 7.63423 15.8591 6.97318L11.0023 2.60207C10.622 2.2598 10.0447 2.25979 9.66437 2.60207Z"
                fill="var(--ui-colors-bg-panel)"
            />
            <path
                d="M8.99542 1.85876C9.75604 1.17425 10.9106 1.17422 11.6713 1.85878L16.5281 6.22989C17.0789 6.72568 17.7938 7.00001 18.5349 7.00001L15.89 7L11.0023 2.60207C10.622 2.2598 10.0447 2.2598 9.66436 2.60207L4.77734 7L2.13171 7.00001C2.87284 7.00001 3.58774 6.72568 4.13861 6.22989L8.99542 1.85876Z"
                fill="var(--ui-colors-border)"
            />
        </svg>
    );
}
