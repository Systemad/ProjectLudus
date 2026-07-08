import { AppShell as AstryxAppShell } from "@astryxdesign/core/AppShell";
import type { ReactNode } from "react";
import { NavigationBar } from "@src/app/navigation-bar";
import { Footer } from "@src/app/footer";

export type AppShellProps = {
    active?: string;
    children: ReactNode;
};

export function AppShell({ active = "home", children }: AppShellProps) {
    return (
        <>
            <AstryxAppShell
                contentPadding={4}
                height="auto"
                topNav={<NavigationBar active={active} />}
            >
                {children}
            </AstryxAppShell>
            <Footer />
        </>
    );
}
