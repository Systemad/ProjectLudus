import { Paper } from "@mantine/core";
import type { PaperProps } from "@mantine/core";
import type React from "react";
import classes from "./Surface.module.css";
type Props = {
    children: React.ReactNode;
    withBorder?: boolean;
    isDark?: boolean;
    filled?: boolean;
};
export function Surface({
    children,
    withBorder,
    isDark,
    filled,
    ...rest
}: Props & PaperProps) {
    const { className, ...other } = rest;
    return (
        <Paper p="md" radius="lg" {...other} className={classes.surface}>
            {children}
        </Paper>
    );
}
