import { PlayIcon } from "@phosphor-icons/react";
import { Box, Float, Motion } from "@yamada-ui/react";

type Props = {
    url: string;
};

export function VideCarouselSlide({ url }: Props) {
    return (
        <Box
            w="xl"
            bgImage={`url(${url})`}
            bgSize="cover"
            bgPosition="center"
            boxSize={"full"}
            position="relative"
            overflow="hidden"
            rounded="xl"
        >
            <Float placement="center-center">
                <Motion
                    as="center"
                    bg="rgba(255,255,255,0.1)"
                    color="white"
                    fontSize="sm"
                    py="1.5"
                    px="3"
                    rounded="xl"
                    whileHover={{ scale: 1.05 }}
                    backdropFilter="blur(4px)"
                    border="2px solid rgba(255,255,255,0.3)"
                >
                    <PlayIcon color="white" fill="white" />
                </Motion>
            </Float>
        </Box>
    );
}
