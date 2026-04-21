import type { ComponentProps, ReactNode } from "react";
import { Box } from "ui";

type BoxWidthProps = ComponentProps<typeof Box>;

type Props = {
    children: ReactNode;
} & Omit<BoxWidthProps, "children">;

export function PageWrapper({
    children,
    maxW = "7xl",
    px = { base: "3", md: "6", xl: "8" },
    py,
    pt,
    pb,
    ...rest
}: Props) {
    return (
        <Box w="full" maxW={maxW} mx="auto" px={px} py={py} pt={pt} pb={pb} {...rest}>
            {children}
        </Box>
    );
}

export default PageWrapper;
