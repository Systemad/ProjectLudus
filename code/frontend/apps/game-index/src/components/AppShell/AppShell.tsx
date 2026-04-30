import { Box } from "ui";
import type { ReactNode } from "react";
import { Footer } from "@src/components/Footer/Footer";
import { NavigationBar } from "@src/features/navigation/components/NavigationBar";
import { appShellContentOffset } from "./layout.constants";

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
                <Box as="main" flex="1">
                    {children}
                </Box>
            ) : (
                <Box as="main" pt={appShellContentOffset} flex="1">
                    {children}
                </Box>
            )}

            <Footer />
        </Box>
    );
}
