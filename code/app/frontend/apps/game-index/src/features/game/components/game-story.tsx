"use client";

import { Text } from "ui";

type Props = {
    storyText: string;
};

export function GameStory({ storyText }: Props) {
    return (
        <Text lineHeight="tall" color="fg.base" lineClamp={5}>
            {storyText}
        </Text>
    );
}
