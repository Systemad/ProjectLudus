type AmbientBackgroundProps = {
    gradient: string;
};

export function AmbientBackground({ gradient }: AmbientBackgroundProps) {
    return (
        <>
            <div
                style={{
                    position: "fixed",
                    inset: 0,
                    zIndex: 0,
                    pointerEvents: "none",
                    backgroundImage: gradient,
                    backgroundRepeat: "no-repeat",
                    backgroundSize: "cover",
                    opacity: 0.42,
                }}
            />

            <div
                style={{
                    position: "fixed",
                    inset: 0,
                    zIndex: 0,
                    pointerEvents: "none",
                    background: "rgba(2, 6, 23, 0.28)",
                    backdropFilter: "saturate(85%)",
                }}
            />
        </>
    );
}
