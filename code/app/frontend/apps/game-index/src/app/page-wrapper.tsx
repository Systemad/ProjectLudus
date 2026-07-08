import type { CSSProperties, ReactNode } from "react";

type Props = {
    children: ReactNode;
    maxWidth?: CSSProperties["maxWidth"];
    paddingInline?: CSSProperties["paddingInline"];
    paddingBlock?: CSSProperties["paddingBlock"];
    paddingTop?: CSSProperties["paddingTop"];
    paddingBottom?: CSSProperties["paddingBottom"];
};

export function PageWrapper({
    children,
    maxWidth = "var(--spacing-9xl, 1128px)",
    paddingInline = "clamp(0.75rem, 3vw, 2rem)",
    paddingBlock,
    paddingTop,
    paddingBottom,
}: Props) {
    return (
        <div
            style={{
                width: "100%",
                maxWidth,
                marginInline: "auto",
                paddingInline,
                paddingBlock,
                paddingTop,
                paddingBottom,
            }}
        >
            {children}
        </div>
    );
}
