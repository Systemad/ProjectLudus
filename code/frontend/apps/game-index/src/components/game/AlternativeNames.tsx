"use client";

import { Box, Tag, Text, Wrap } from "ui";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";

type Props = {
    names: string[];
};

export default function AlternativeNames({ names }: Props) {
    return (
        <Box>
            <Text {...sectionLabelStyle} mb={3}>
                Alternative names
            </Text>
            {names.length > 0 ? (
                <Wrap gap="xs">
                    {names.map((name) => (
                        <Tag
                            key={name}
                            variant="surface"
                            colorScheme="gray"
                            textTransform="none"
                            maxW="full"
                            whiteSpace="normal"
                            wordBreak="break-word"
                        >
                            {name}
                        </Tag>
                    ))}
                </Wrap>
            ) : (
                <Text color="fg.subtle">No alternative names available.</Text>
            )}
        </Box>
    );
}
