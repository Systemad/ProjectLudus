import { Box } from "@yamada-ui/react";
type Props = {
    children: React.ReactNode;
};

export const AccordionCardItemCard = ({ children }: Props) => {
    return (
        <Box
            borderRadius={"lg"}
            bg={["blackAlpha.50", "whiteAlpha.100"]}
            shadow="none"
            h="5xs"
            position={"relative"}
        >
            {children}
        </Box>
    );
};
