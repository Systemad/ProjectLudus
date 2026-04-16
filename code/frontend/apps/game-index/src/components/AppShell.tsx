import { Box } from "ui";
import type { ReactNode } from "react";
import { Footer } from "./Footer";
import { NavigationBar } from "./NavigationBar";

export type AppShellProps = {
    active?: string;
    children: ReactNode;
    fullBleed?: boolean;
};

export function AppShell({ active = "home", children, fullBleed = false }: AppShellProps) {
    return (
        <Box minH="dvh" bg="bg.base" color="fg.base" display="flex" flexDirection="column">
            <NavigationBar active={active} />

            {fullBleed ? (
                <Box flex="1">{children}</Box>
            ) : (
                <Box pt={{ base: "24", md: "28" }} flex="1">
                    {children}
                </Box>
            )}

            <Footer />
        </Box>
    );
}

export default AppShell;
