"use client";

import { Box, Button, ChevronRightIcon, Text, Collapse, useBoolean, Container } from "ui";
import { sectionLabelStyle } from "@src/utils/sectionTextStyles";

type Props = {
    storyText: string;
};

export function GameStory({ storyText }: Props) {
    const isLong = storyText.length > 280;
    const [open, { toggle }] = useBoolean();
    return (
        <Container.Root rounded="2xl" variant="subtle">
            <Container.Header>
                <Text {...sectionLabelStyle}>Story</Text>
            </Container.Header>

            <Container.Body>
                <Collapse open={open} startingHeight={150}>
                    <Text color="fg.subtle" lineHeight="tall">
                        {storyText}
                    </Text>
                </Collapse>

                {isLong ? (
                    <Box textAlign="center">
                        <Button
                            variant="ghost"
                            size="sm"
                            disableRipple
                            onClick={toggle}
                            endIcon={
                                <ChevronRightIcon
                                    boxSize="4"
                                    transitionProperty="transform"
                                    transitionDuration="moderate"
                                    transform={open ? "rotate(90deg)" : "rotate(0deg)"}
                                />
                            }
                        >
                            {open ? "Read more" : "Show less"}
                        </Button>
                    </Box>
                ) : null}
            </Container.Body>
        </Container.Root>
    );
}
