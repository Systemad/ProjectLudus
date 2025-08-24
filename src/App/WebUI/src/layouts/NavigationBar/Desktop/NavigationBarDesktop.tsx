import {
    AnimatePresence,
    Box,
    Button,
    HStack,
    mergeRefs,
    Motion,
    forwardRef,
    Spacer,
    useHover,
    type ButtonProps,
} from "@yamada-ui/react";
import { ThemeButton } from "../ThemeButton";
import type { NavBarItem } from "../NavigationBar";
import { memo, useMemo } from "react";
import { Link, useLocation } from "@tanstack/react-router";

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
            <HStack as={Motion} gap="xs">
                <DesktopNavItems items={items} />
            </HStack>
            <Spacer />
            <ThemeButton />
        </HStack>
    );
};

type DesktopNavItemsProps = {
    items: NavBarItem[];
};
const DesktopNavItems = memo(({ items }: DesktopNavItemsProps) => {
    return (
        <HStack as={Motion} gap={0}>
            {items.map((item) => (
                <Motion>
                    <NavigationBarButton label={item.label} url={item.url} />
                </Motion>
            ))}
        </HStack>
    );
});

interface NavigationBarButtonProps extends ButtonProps {
    label: string;
    url: string;
}

// TODO: fix flicker, active
export const NavigationBarButton = memo(
    forwardRef<NavigationBarButtonProps, "a">(
        ({ label, url, ...props }, ref) => {
            const location = useLocation();
            const active =
                location.pathname === url ||
                location.pathname.startsWith(url + "/");

            const { hovered, ref: hoverRef } = useHover();
            const mergedRef = mergeRefs(ref, hoverRef);

            const showIndicator = useMemo(() => {
                return hovered || active;
            }, [hovered, active]);

            return (
                <Box position="relative">
                    <AnimatePresence mode="wait">
                        {showIndicator && (
                            <NavigationBarHoverIndicator key="indicator" />
                        )}
                    </AnimatePresence>
                    <Button
                        as={Link}
                        to={url}
                        zIndex="1"
                        px="md"
                        fontSize="lg"
                        position="relative"
                        variant={"solid"}
                        colorScheme={"solid"}
                        ref={mergedRef}
                        {...props}
                    >
                        {label}
                    </Button>
                </Box>
            );
        }
    )
);

export const NavigationBarHoverIndicator = () => {
    return (
        <Motion
            animate={{ opacity: 1 }}
            backdropBlur="xl"
            backdropFilter="auto"
            bgGradient="secondaryGradient"
            borderRadius="full"
            data-testid="navbar-hover-indicator"
            exit={{ opacity: 0.25, transition: { duration: 0.4 } }}
            height={8}
            initial={{ opacity: 0 }}
            inset={0}
            layoutId="navigation-bar-indicator"
            position="absolute"
            px="md"
            transform={"translateY(-0.75px)"}
            transition={{
                layout: { type: "spring", stiffness: 275, damping: 25 },
                default: { duration: 0.3, ease: "easeOut" },
            }}
            zIndex="0"
        />
    );
};
