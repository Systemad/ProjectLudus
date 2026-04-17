import { Box, Button, Flex, Text } from "ui";

type PeriodNavigatorProps = {
    periodLabel: string;
    onPrevious: () => void;
    onNext: () => void;
};

export function PeriodNavigator({ periodLabel, onPrevious, onNext }: PeriodNavigatorProps) {
    return (
        <Flex gap="xs" align="center" w="full">
            <Button size="sm" variant="outline" onClick={onPrevious}>
                Prev
            </Button>
            <Box
                flex="1"
                minW={0}
                rounded="md"
                borderWidth="1px"
                borderColor="border.subtle"
                px="sm"
                py="xs"
            >
                <Text fontSize="sm" fontWeight="medium" textAlign="center" lineClamp={1}>
                    {periodLabel}
                </Text>
            </Box>
            <Button size="sm" variant="outline" onClick={onNext}>
                Next
            </Button>
        </Flex>
    );
}
