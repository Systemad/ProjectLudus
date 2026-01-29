"use client";

import * as React from "react";
import { NavigationMenu as NavigationMenuPrimitive } from "@base-ui/react/navigation-menu";
import { ChevronDownIcon, styled } from "@packages/ui";

const NavigationMenuRootStyled = styled(NavigationMenuPrimitive.Root, {
    base: {
        backgroundColor: "bg.float",
        borderRadius: "0.5rem",
        padding: "0.25rem",
        minWidth: "max-content",
    },
});

const NavigationMenuListStyled = styled(NavigationMenuPrimitive.List, {
    base: {
        display: "flex",
        listStyle: "none",
        padding: 0,
        margin: 0,
    },
});

const NavigationMenuArrowStyled = styled(NavigationMenuPrimitive.Arrow, {
    base: {
        display: "flex",
        //color: "bg.float",
        //backgroundColor: "bg.float",
        //position: "absolute",
        transition: "left calc(var(--duration)) var(--easing)",

        "&[data-side='top']": {
            bottom: "-8px",
            rotate: "180deg",
        },

        "&[data-side='bottom']": {
            top: "-8px",
        },

        "&[data-side='left']": {
            right: "-12px",
            rotate: "90deg",
        },

        "&[data-side='right']": {
            left: "-12px",
            rotate: "-90deg",
        },
    },
});

// styles.ArrowFill
const ArrowFill = styled("path", {
    base: {
        fill: "canvas",
    },
});

// styles.ArrowOuterStroke
const ArrowOuterStroke = styled("path", {
    base: {
        "@media (prefers-color-scheme: light)": {
            fill: "gray.200",
        },
    },
});

// styles.ArrowInnerStroke
const ArrowInnerStroke = styled("path", {
    base: {
        "@media (prefers-color-scheme: dark)": {
            fill: "gray.300",
        },
    },
});

export function ArrowSvg(props: React.ComponentProps<"svg">) {
    return (
        <svg width="20" height="10" viewBox="0 0 20 10" fill="none" {...props}>
            <ArrowFill d="M9.66437 2.60207L4.80758 6.97318C4.07308 7.63423 3.11989 8 2.13172 8H0V10H20V8H18.5349C17.5468 8 16.5936 7.63423 15.8591 6.97318L11.0023 2.60207C10.622 2.2598 10.0447 2.25979 9.66437 2.60207Z" />

            <ArrowOuterStroke d="M8.99542 1.85876C9.75604 1.17425 10.9106 1.17422 11.6713 1.85878L16.5281 6.22989C17.0789 6.72568 17.7938 7.00001 18.5349 7.00001L15.89 7L11.0023 2.60207C10.622 2.2598 10.0447 2.2598 9.66436 2.60207L4.77734 7L2.13171 7.00001C2.87284 7.00001 3.58774 6.72568 4.13861 6.22989L8.99542 1.85876Z" />

            <ArrowInnerStroke d="M10.3333 3.34539L5.47654 7.71648C4.55842 8.54279 3.36693 9 2.13172 9H0V8H2.13172C3.11989 8 4.07308 7.63423 4.80758 6.97318L9.66437 2.60207C10.0447 2.25979 10.622 2.2598 11.0023 2.60207L15.8591 6.97318C16.5936 7.63423 17.5468 8 18.5349 8H20V9H18.5349C17.2998 9 16.1083 8.54278 15.1901 7.71648L10.3333 3.34539Z" />
        </svg>
    );
}

const NavigationMenuTriggerStyled = styled(NavigationMenuPrimitive.Trigger, {
    base: {
        display: "flex",
        alignItems: "center",
        gap: "0.375rem",
        height: "2.5rem",
        padding: "0 0.875rem",
        borderRadius: "0.375rem",
        border: "none",
        backgroundColor: "var(--color-gray-50)",
        fontWeight: 500,
        cursor: "pointer",

        "&[data-popup-open]": {
            backgroundColor: "var(--color-gray-100)",
        },

        "&:focus-visible": {
            outline: "2px solid var(--color-blue)",
            outlineOffset: "-1px",
        },
    },
});

const NavigationMenuIconStyled = styled(NavigationMenuPrimitive.Icon, {
    base: {
        transition: "transform 0.2s ease",
        "&[data-popup-open]": {
            transform: "rotate(180deg)",
        },
    },
});

const NavigationMenuContentStyled = styled(NavigationMenuPrimitive.Content, {
    base: {
        boxSizing: "border-box",
        padding: "sm",

        transition:
            "opacity calc(var(--duration) * 0.5) ease, transform var(--duration) var(--easing)",

        "&[data-starting-style]": {
            opacity: 0,
            "&[data-activation-direction='left']": {
                transform: "translateX(-40px)",
            },
            "&[data-activation-direction='right']": {
                transform: "translateX(40px)",
            },
        },

        "&[data-ending-style]": {
            opacity: 0,
            "&[data-activation-direction='left']": {
                transform: "translateX(40px)",
            },
            "&[data-activation-direction='right']": {
                transform: "translateX(-40px)",
            },
        },
    },
});

const NavigationMenuPositionerStyled = styled(
    NavigationMenuPrimitive.Positioner,
    {
        base: {
            "--duration": "0.35s",
            "--easing": "cubic-bezier(0.22, 1, 0.36, 1)",

            boxSizing: "border-box",
            width: "var(--positioner-width)",
            height: "var(--positioner-height)",
            maxWidth: "var(--available-width)",

            transitionProperty: "top, left, right, bottom, width, height",
            transitionDuration: "var(--duration)",
            transitionTimingFunction: "var(--easing)",

            "&[data-instant]": {
                transition: "none",
            },
        },
    }
);

const NavigationMenuPopupStyled = styled(NavigationMenuPrimitive.Popup, {
    base: {
        backgroundColor: "bg.float",
        border: "1px solid var(--color-border-muted)",
        borderRadius: "0.5rem",
        boxShadow: "sm",

        width: "var(--popup-width)",
        height: "var(--popup-height)",

        transformOrigin: "var(--transform-origin)",
        transitionProperty: "opacity, transform, width, height",
        transitionDuration: "var(--duration)",
        transitionTimingFunction: "var(--easing)",

        "&[data-starting-style]": {
            opacity: 0,
            transform: "scale(0.95)",
        },

        "&[data-ending-style]": {
            opacity: 0,
            transform: "scale(0.95)",
            transitionDuration: "0.15s",
        },
    },
});

const NavigationMenuViewportStyled = styled(NavigationMenuPrimitive.Viewport, {
    base: {
        position: "relative",
        overflow: "hidden",
        width: "100%",
        height: "100%",
    },
});

const NavigationMenuLinkStyled = styled(NavigationMenuPrimitive.Link, {
    base: {
        boxSizing: "border-box",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        gap: "0.375rem",
        height: "2.5rem",
        padding: "0 0.875rem",
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

const NavigationMenu = React.forwardRef<
    React.ElementRef<typeof NavigationMenuRootStyled>,
    React.ComponentPropsWithoutRef<typeof NavigationMenuRootStyled>
>(({ children, ...props }, ref) => {
    return (
        <NavigationMenuRootStyled ref={ref} {...props}>
            {children}

            <NavigationMenuPrimitive.Portal>
                <NavigationMenuPositionerStyled
                    sideOffset={10}
                    collisionPadding={{
                        top: 5,
                        bottom: 5,
                        left: 20,
                        right: 20,
                    }}
                    collisionAvoidance={{ side: "none" }}
                >
                    <NavigationMenuPopupStyled>
                        <NavigationMenuArrowStyled>
                            <ArrowSvg />
                        </NavigationMenuArrowStyled>

                        <NavigationMenuViewportStyled />
                    </NavigationMenuPopupStyled>
                </NavigationMenuPositionerStyled>
            </NavigationMenuPrimitive.Portal>
        </NavigationMenuRootStyled>
    );
});
NavigationMenu.displayName = "NavigationMenu";

const NavigationMenuList = React.forwardRef<
    React.ElementRef<typeof NavigationMenuListStyled>,
    React.ComponentPropsWithoutRef<typeof NavigationMenuListStyled>
>((props, ref) => <NavigationMenuListStyled ref={ref} {...props} />);
NavigationMenuList.displayName = "NavigationMenuList";

const NavigationMenuItem = NavigationMenuPrimitive.Item;

const NavigationMenuTrigger = React.forwardRef<
    React.ElementRef<typeof NavigationMenuTriggerStyled>,
    React.ComponentPropsWithoutRef<typeof NavigationMenuTriggerStyled>
>(({ children, ...props }, ref) => (
    <NavigationMenuTriggerStyled ref={ref} {...props}>
        {children}
        <NavigationMenuIconStyled>
            <ChevronDownIcon />
        </NavigationMenuIconStyled>
    </NavigationMenuTriggerStyled>
));
NavigationMenuTrigger.displayName = "NavigationMenuTrigger";

const NavigationMenuContent = React.forwardRef<
    React.ElementRef<typeof NavigationMenuContentStyled>,
    React.ComponentPropsWithoutRef<typeof NavigationMenuContentStyled>
>((props, ref) => <NavigationMenuContentStyled ref={ref} {...props} />);
NavigationMenuContent.displayName = "NavigationMenuContent";

const NavigationMenuLink = NavigationMenuLinkStyled;

export {
    NavigationMenu,
    NavigationMenuList,
    NavigationMenuItem,
    NavigationMenuTrigger,
    NavigationMenuContent,
    NavigationMenuLink,
};
