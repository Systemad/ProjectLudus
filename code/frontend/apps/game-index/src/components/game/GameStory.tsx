"use client";

import { useState } from "react";
import { Box, Button, ChevronRightIcon, Text } from "ui";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";

type Props = {
    storyText: string;
};

export function GameStory({ storyText }: Props) {
    const [expanded, setExpanded] = useState(false);
    const isLong = storyText.length > 280;

    return (
        <Box>
            <Text {...sectionLabelStyle} mb={3}>
                Story
            </Text>
            <Text color="fg.subtle" lineHeight="tall" lineClamp={expanded ? undefined : 5}>
                {storyText}
            </Text>
            {isLong ? (
                <Box textAlign="right" mt="2">
                    <Button
                        variant="ghost"
                        size="sm"
                        disableRipple
                        onClick={() => setExpanded((prev) => !prev)}
                        endIcon={
                            <ChevronRightIcon
                                boxSize="4"
                                transitionProperty="transform"
                                transitionDuration="moderate"
                                transform={expanded ? "rotate(90deg)" : "rotate(0deg)"}
                            />
                        }
                    >
                        {expanded ? "Show less" : "Read more"}
                    </Button>
                </Box>
            ) : null}
        </Box>
    );
}

export default GameStory;
