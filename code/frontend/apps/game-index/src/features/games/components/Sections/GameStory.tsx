"use client";

import { useEffect, useRef, useState } from "react";
import { Box, Button, ChevronRightIcon, Text, Collapse, useBoolean, Container } from "ui";

type Props = {
    storyText: string;
};

export function GameStory({ storyText }: Props) {
    const collapseHeight = 150;
    const textRef = useRef<HTMLParagraphElement>(null);
    const [canExpand, setCanExpand] = useState(false);
    const [open, { toggle }] = useBoolean();

    useEffect(() => {
        const node = textRef.current;

        if (!node) {
            setCanExpand(false);
            return;
        }

        const updateOverflowState = () => {
            setCanExpand(node.scrollHeight > collapseHeight + 1);
        };

        updateOverflowState();

        const observer = new ResizeObserver(updateOverflowState);
        observer.observe(node);

        return () => observer.disconnect();
    }, [storyText]);

    return (
        <Container.Root
            rounded="2xl"
            variant="surface"
            colorScheme="gray"
            bg="bg.panel"
            borderWidth="1px"
            borderColor="border.subtle"
        >
            <Container.Header>
                <Text
                    fontSize="xl"
                    fontWeight="semibold"
                    textTransform="uppercase"
                    letterSpacing="wide"
                    color="colorScheme.fg"
                >
                    Story
                </Text>
            </Container.Header>

            <Container.Body>
                <Collapse open={open} startingHeight={collapseHeight}>
                    <Text ref={textRef} lineHeight="tall" color="colorScheme.fg">
                        {storyText}
                    </Text>
                </Collapse>

                {canExpand ? (
                    <Box textAlign="center">
                        <Button
                            variant="ghost"
                            colorScheme="gray"
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
                            {open ? "Show less" : "Read more"}
                        </Button>
                    </Box>
                ) : null}
            </Container.Body>
        </Container.Root>
    );
}
