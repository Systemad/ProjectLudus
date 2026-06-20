import { Box } from "ui";
import type { ReactNode } from "react";
import { Footer } from "@src/app/footer";
import { NavigationBar } from "@src/app/navigation-bar";

export type AppShellProps = {
    active?: string;
    children: ReactNode;
};

export function AppShell({ active = "home", children }: AppShellProps) {
    return (
        <Box minH="dvh" color="fg.base" display="flex" flexDirection="column">
            <NavigationBar active={active} />
            <Box as="main" flex="1">
                {children}
            </Box>
            <Footer />
        </Box>
    );
}
