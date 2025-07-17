import { Box, Flex } from "@yamada-ui/react";
import AppleIcon from "~/icons/Consoles/AppleIcon";
import LinuxIcon from "~/icons/Consoles/LinuxIcons";
import PlaystationIcon from "~/icons/Consoles/PlaystationIcon";
import WindowsIcon from "~/icons/Consoles/WindowsIcon";
import XboxIcon from "~/icons/Consoles/XboxIcon";
import { AccordionCardItemCard } from "./AccordionItemCard";

type Props = {
    text: string;
};

export const PlatformAccordionCardItem = ({ text }: Props) => {
    const textNorm = text.toLocaleLowerCase();

    let icon = null;

    if (textNorm.includes("xbox")) {
        icon = <XboxIcon />;
    } else if (textNorm.includes("playstation")) {
        icon = <PlaystationIcon />;
    } else if (textNorm.includes("linux")) {
        icon = <LinuxIcon />;
    } else if (textNorm.includes("nintendo")) {
        icon = <PlaystationIcon />;
    } else if (textNorm.includes("windows")) {
        icon = <WindowsIcon />;
    } else if (textNorm.includes("mac")) {
        icon = <AppleIcon />;
    }

    return (
        <AccordionCardItemCard>
            <Box position={"absolute"} inset={0} opacity={0}></Box>
            <Flex h="full" gap="sm" padding={4} alignItems={"center"}>
                {icon}
                {text}
            </Flex>
        </AccordionCardItemCard>
    );
};
