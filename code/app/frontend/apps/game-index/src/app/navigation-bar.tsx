import { TopNav, TopNavHeading, TopNavItem } from "@astryxdesign/core/TopNav";
import { NavIcon } from "@astryxdesign/core/NavIcon";
import { Button } from "@astryxdesign/core/Button";
import { Icon } from "@astryxdesign/core/Icon";
import { HStack } from "@astryxdesign/core/HStack";
import {
    CubeIcon,
    MagnifyingGlassIcon,
    UserCircleIcon,
    BellIcon,
    BuildingOfficeIcon,
    RocketLaunchIcon,
} from "@heroicons/react/24/outline";

type NavigationBarProps = {
    active?: string;
};

const NAV_ITEMS = [
    {
        id: "search",
        label: "Search",
        isMegaMenu: true,
        items: [
            {
                title: "Games",
                description: "Search by name, genre, or keyword",
                href: "/games/search",
                icon: MagnifyingGlassIcon,
            },
            {
                title: "Companies",
                description: "Search for publishers and developers",
                href: "/companies/search",
                icon: BuildingOfficeIcon,
            },
        ],
    },
    {
        id: "events",
        label: "Events",
        isMegaMenu: true,
        items: [
            {
                title: "Upcoming Events",
                description: "Browse gaming events and conferences",
                href: "/events",
                icon: RocketLaunchIcon,
            },
        ],
    },
    {
        id: "calendar",
        label: "Calendar",
        href: "/calendar",
    },
    {
        id: "companies",
        label: "Companies",
        href: "/companies/search",
    },
];

export function NavigationBar({ active: _active }: NavigationBarProps) {
    return (
        <TopNav
            label="Main navigation"
            heading={
                <TopNavHeading
                    heading="Game-Index"
                    href="/"
                    logo={<NavIcon icon={<Icon icon={CubeIcon} size="sm" />} />}
                />
            }
            centerContent={
                <>
                    {NAV_ITEMS.map((item) =>
                        "items" in item && item.isMegaMenu ? (
                            null
                        ) : "href" in item ? (
                            <TopNavItem
                                key={item.id}
                                label={item.label}
                                href={item.href}
                                isSelected={_active === item.id}
                            />
                        ) : null
                    )}
                </>
            }
            endContent={
                <HStack gap={2} vAlign="center">
                    <Button
                        label="Search"
                        variant="ghost"
                        icon={<Icon icon={MagnifyingGlassIcon} size="sm" />}
                        isIconOnly
                    />
                    <Button
                        label="Notifications"
                        variant="ghost"
                        icon={<Icon icon={BellIcon} size="sm" />}
                        isIconOnly
                    />
                    <Button
                        label="Profile"
                        variant="ghost"
                        icon={<Icon icon={UserCircleIcon} size="sm" />}
                        isIconOnly
                    />
                </HStack>
            }
        />
    );
}
