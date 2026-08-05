import { TopNav, TopNavHeading, TopNavItem } from "@astryxdesign/core/TopNav";
import { Button } from "@astryxdesign/core/Button";
import { Icon } from "@astryxdesign/core/Icon";
import { HStack } from "@astryxdesign/core/HStack";
import { useRouterState } from "@tanstack/react-router";
import { presentationStyles } from "@src/app/presentation-styles";
import {
    Bars3Icon,
    MagnifyingGlassIcon,
} from "@heroicons/react/24/outline";

const NAV_ITEMS = [
    { id: "calendar", label: "Calendar", href: "/calendar" },
    { id: "events", label: "Events", href: "/events" },
    { id: "companies", label: "Companies", href: "/companies/search" },
];

export function NavigationBar({ onMobileNavOpen }: { onMobileNavOpen: () => void }) {
    const pathname = useRouterState({ select: (state) => state.location.pathname });

    return (
        <TopNav
            xstyle={presentationStyles.navigationSurface}
            label="Main navigation"
            heading={
                <TopNavHeading
                    heading="Game-Index"
                    href="/"
                />
            }
            centerContent={
                <HStack gap={1} vAlign="center">
                    {NAV_ITEMS.map((item) => (
                        <TopNavItem
                            key={item.id}
                            label={item.label}
                            href={item.href}
                            isSelected={pathname === item.href || pathname.startsWith(`${item.href}/`)}
                        />
                    ))}
                </HStack>
            }
            endContent={
                <HStack gap={2} vAlign="center">
                    <Button
                        label="Search"
                        variant="ghost"
                        href="/games/search"
                        icon={<Icon icon={MagnifyingGlassIcon} size="sm" />}
                        isIconOnly
                    />
                    <Button
                        label="Open navigation"
                        variant="ghost"
                        icon={<Icon icon={Bars3Icon} size="sm" />}
                        onClick={onMobileNavOpen}
                        isIconOnly
                        xstyle={presentationStyles.mobileNavigationToggle}
                    />
                </HStack>
            }
        />
    );
}
