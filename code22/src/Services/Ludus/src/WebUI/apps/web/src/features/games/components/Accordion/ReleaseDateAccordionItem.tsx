import { Flex, Text } from "@yamada-ui/react";
import { AccordionCardItemCard } from "./AccordionItemCard";
import type { ReleaseDate } from "~/gen";

type Props = {
    releaseDate: ReleaseDate;
};

export const ReleaseDateAccordionItem = ({ releaseDate }: Props) => {
    return (
        <AccordionCardItemCard>
            <Flex direction="column" align="center" gap="2xs" p={4}>
                <Text lineClamp={1} fontWeight="bold" fontSize="md">
                    {releaseDate.human}
                </Text>
                <Text lineClamp={1} fontSize="sm" color="muted">
                    {releaseDate.platform.name}
                </Text>
            </Flex>
        </AccordionCardItemCard>
    );
};
