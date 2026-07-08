"use client";

import { Text } from "@astryxdesign/core/Text";

type Props = {
    storyText: string;
};

export function GameStory({ storyText }: Props) {
    return (
        <Text style={{lineHeight: 1.8}} maxLines={5}>
            {storyText}
        </Text>
    );
}
