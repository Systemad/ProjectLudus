import { PreviewCard as Base } from "@base-ui/react/preview-card";
import { styled } from "ui";

export const PreviewCardRoot = Base.Root;
export const PreviewCardTrigger = styled(Base.Trigger, {
    base: {
        cursor: "pointer",
        _focusVisible: {
            //outline: "2px solid",
            //outlineColor: "blue.500",
            //rounded: "sm",
        },
    },
    defaultProps: {
        // @ts-expect-error — delay is a valid Base UI Trigger prop
        delay: 250,
        //closeDelay: 0,
    },
});
export const PreviewCardPortal = Base.Portal;
export const PreviewCardPositioner = Base.Positioner;
export const PreviewCardPopup = styled(Base.Popup, {
    base: {
        //bg: "bg.panel",
        //rounded: "lg",
        //boxShadow: "lg",
        //borderWidth: "1px",
        //borderColor: "border",
        //p: "md",
        outline: "none",
        transformOrigin: "var(--transform-origin)",
        transitionDuration: "moderate",
        transitionProperty: "common",
        _startingStyle: {
            opacity: 0,
            transform: "scale(0.95)",
        },
    },
});
export const PreviewCardArrow = styled(Base.Arrow, {
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
export const PreviewCardViewport = Base.Viewport;
export const PreviewCardBackdrop = Base.Backdrop;
export const createPreviewCardHandle = Base.createHandle;

export function PreviewCardArrowSvg(props: React.ComponentProps<"svg">) {
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
