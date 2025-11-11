import { Container } from "@yamada-ui/react";

import { NavigationBarDesktop } from "./Desktop/NavigationBarDesktop";

export type NavBarItem = {
    label: string;
    url: string;
};

const navItems: NavBarItem[] = [
    { label: "Games", url: "/games" },
    { label: "Calendar", url: "/calender" },
    { label: "Popular", url: "/popular" },
];

export const NavigationBar = () => {
    return (
        <Container
            as="nav"
            centerContent
            gap="0"
            layerStyle="container"
            pb={{ base: "0", sm: "0", lg: "0" }}
            position="sticky"
            pt={{ base: "md", lg: "lg" }}
            top="0"
            w="full"
            zIndex={100}
        >
            <NavigationBarDesktop items={navItems} />
        </Container>
    );
};
