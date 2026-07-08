"use client";

import { Text } from "@astryxdesign/core/Text";
import { Badge } from "@astryxdesign/core/Badge";

type Props = {
    names: string[];
};

export function AlternativeNames({ names }: Props) {
    return (
        <div>
            <Text style={{fontSize: "1.25rem", fontWeight: 600, textTransform: "uppercase", letterSpacing: "0.05em", marginBottom: "0.75rem"}}>
                Alternative names
            </Text>
            {names.length > 0 ? (
                <div style={{ display: "flex", flexWrap: "wrap", gap: "0.25rem" }}>
                    {names.map((name) => (
                        <Badge
                            key={name}
                            variant="neutral"
                            label={name}
                            style={{
                                textTransform: "none",
                                maxWidth: "100%",
                                whiteSpace: "normal",
                                wordBreak: "break-word",
                            }}
                        />
                    ))}
                </div>
            ) : (
                <Text style={{color: "var(--fg-tertiary)"}}>No alternative names available.</Text>
            )}
        </div>
    );
}
