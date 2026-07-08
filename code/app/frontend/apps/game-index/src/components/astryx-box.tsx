import * as stylex from "@stylexjs/stylex";
import type { ReactNode } from "react";

type BoxProps = {
    children: ReactNode;
    as?: "div" | "section" | "main" | "article" | "span";
    className?: string;
    style?: React.CSSProperties;
    onClick?: () => void;
};

const styles = stylex.create({
    base: {
        backgroundColor: "var(--color-background-surface)",
        color: "var(--color-text-primary)",
        padding: "var(--spacing-4)",
        borderRadius: "var(--radius-container)",
    },
});

export const Box = ({
    children,
    as: Tag = "div",
    className,
    style,
    onClick,
}: BoxProps) => {
    return (
        <Tag
            {...stylex.props(styles.base)}
            className={className}
            style={style}
            onClick={onClick}
        >
            {children}
        </Tag>
    );
};
