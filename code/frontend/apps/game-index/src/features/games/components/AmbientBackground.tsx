import { Box } from "ui";

type AmbientBackgroundProps = {
    gradient: string;
};

export default function AmbientBackground({ gradient }: AmbientBackgroundProps) {
    return (
        <>
            <Box
                position="fixed"
                inset={0}
                zIndex={0}
                pointerEvents="none"
                backgroundImage={gradient}
                backgroundRepeat="no-repeat"
                backgroundSize="cover"
                opacity={0.42}
            />

            <Box
                position="fixed"
                inset={0}
                zIndex={0}
                pointerEvents="none"
                bg="transparentize(gray.950, 28%)"
                backdropFilter="saturate(85%)"
            />
        </>
    );
}
