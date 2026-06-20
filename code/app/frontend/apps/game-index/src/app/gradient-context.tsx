import { createContext, useContext, useMemo, useState, type ReactNode } from "react";

export const defaultShellGradient =
    "linear-gradient(180deg, rgba(15, 18, 28, 1), rgba(8, 10, 18, 1))";

type GradientContextValue = {
    gradient: string;
    setGradient: (gradient: string) => void;
    resetGradient: () => void;
};

const GradientContext = createContext<GradientContextValue | undefined>(undefined);

export function GradientProvider({ children }: { children: ReactNode }) {
    const [gradient, setGradient] = useState(defaultShellGradient);

    const value = useMemo(
        () => ({
            gradient,
            setGradient,
            resetGradient: () => setGradient(defaultShellGradient),
        }),
        [gradient],
    );

    return <GradientContext.Provider value={value}>{children}</GradientContext.Provider>;
}

export function useShellGradient() {
    const context = useContext(GradientContext);
    if (!context) {
        throw new Error("useShellGradient must be used inside GradientProvider");
    }
    return context;
}
