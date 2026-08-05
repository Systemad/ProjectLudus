"use client";

import { AppShell as AstryxAppShell } from "@astryxdesign/core/AppShell";
import { MobileNav } from "@astryxdesign/core/MobileNav";
import { SideNavItem, SideNavSection } from "@astryxdesign/core/SideNav";
import type { ReactNode } from "react";
import { useState } from "react";
import { NavigationBar } from "@src/app/navigation-bar";
import { Footer } from "@src/app/footer";
import { presentationStyles } from "@src/app/presentation-styles";
import * as stylex from "@stylexjs/stylex";

export type AppShellProps = {
    active?: string;
    children: ReactNode;
};

const NAV_ITEMS = [
    { label: "Calendar", href: "/calendar" },
    { label: "Events", href: "/events" },
    { label: "Companies", href: "/companies/search" },
];

export function AppShell({ children }: AppShellProps) {
    const [isMobileNavOpen, setIsMobileNavOpen] = useState(false);

    return (
        <AstryxAppShell
            variant="wash"
            contentPadding={4}
            height="fill"
            topNav={<NavigationBar onMobileNavOpen={() => setIsMobileNavOpen(true)} />}
            mobileNav={
                <MobileNav isOpen={isMobileNavOpen} onOpenChange={setIsMobileNavOpen} header="Game-Index">
                    <SideNavSection title="Explore">
                        {NAV_ITEMS.map((item) => <SideNavItem key={item.href} label={item.label} href={item.href} />)}
                    </SideNavSection>
                </MobileNav>
            }
        >
            <div {...stylex.props(presentationStyles.shell)}>
                <div {...stylex.props(presentationStyles.content)}>
                    {children}
                </div>
                <Footer />
            </div>
        </AstryxAppShell>
    );
}
