import {
    HStack,
    Spacer,
    SegmentedControl,
    SegmentedControlButton,
    Box,
} from "@yamada-ui/react";
import { ThemeButton } from "../ThemeButton";
import type { NavBarItem } from "../NavigationBar";
import { Link, useRouterState } from "@tanstack/react-router";
import { useDeferredValue, useState } from "react";
import { GameControllerIcon } from "@phosphor-icons/react";
import { AuthButton } from "~/features/auth/AuthButton";

type Props = {
    items: NavBarItem[];
};
export const NavigationBarDesktop = ({ items }: Props) => {
    return (
        <HStack
            backdropBlur="lg"
            backdropFilter="auto"
            bg={["blackAlpha.50", "whiteAlpha.100"]}
            rounded="3xl"
            display={{ base: "none", md: "flex" }}
            gap={0}
            position="relative"
            px="lg"
            py="md"
            width="7xl"
            zIndex={1001}
        >
            <HStack gap="xs">
                <Box
                    as={Link}
                    href="/"
                    borderRadius="50%"
                    position="relative"
                    display="flex"
                    alignItems="center"
                    justifyContent="center"
                >
                    <GameControllerIcon size={28} />
                </Box>
                <DesktopNavItems items={items} />
            </HStack>
            <Spacer />
            <Box>
                <ThemeButton />
                <AuthButton />
            </Box>
        </HStack>
    );
};

type DesktopNavItemsProps = {
    items: NavBarItem[];
};
const DesktopNavItems = ({ items }: DesktopNavItemsProps) => {
    const routerState = useRouterState();

    const activeValue =
        items.find(
            (item) =>
                routerState.location.pathname === item.url ||
                routerState.location.pathname.startsWith(item.url)
        )?.url ?? "__none__";

    const [hoveredValue, setHoveredValue] = useState<string | null>(null);
    const def = useDeferredValue(hoveredValue);
    return (
        <SegmentedControl
            p="0"
            variant="rounded"
            value={def ?? activeValue}
            size="lg"
        >
            {items.map(({ label, url }) => (
                <SegmentedControlButton
                    key={url}
                    preload="intent"
                    as={Link}
                    to={url}
                    value={url}
                    onMouseEnter={() => setHoveredValue(url)}
                    onMouseLeave={() => setHoveredValue(null)}
                >
                    {label}
                </SegmentedControlButton>
            ))}

            <SegmentedControlButton
                value="__none__"
                style={{ display: "none" }}
            >
                none
            </SegmentedControlButton>
        </SegmentedControl>
    );
};
