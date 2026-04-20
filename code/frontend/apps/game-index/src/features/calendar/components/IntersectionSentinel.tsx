"use client";
import { useEffect, useRef, useCallback } from "react";
import { Box } from "ui";

type IntersectionSentinelProps = {
    onVisible: () => void;
};

export function IntersectionSentinel({ onVisible }: IntersectionSentinelProps) {
    const sentinelRef = useRef<HTMLDivElement | null>(null);

    const handleIntersection = useCallback(
        (entries: IntersectionObserverEntry[]) => {
            for (const entry of entries) {
                if (entry.isIntersecting) {
                    onVisible();
                }
            }
        },
        [onVisible],
    );

    useEffect(() => {
        if (!sentinelRef.current) return;

        const observer = new IntersectionObserver(handleIntersection, {
            root: null,
            rootMargin: "200px",
            threshold: 0.1,
        });

        observer.observe(sentinelRef.current);

        return () => {
            observer.disconnect();
        };
    }, [handleIntersection]);

    return <Box ref={sentinelRef} minH="4" />;
}
