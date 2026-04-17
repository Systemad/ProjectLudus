import { Box } from "ui";
import { ReleaseRadar } from "@src/components/release-radar/ReleaseRadar";

type ReleaseRadarSearchLayoutProps = Record<string, never>;

export function ReleaseRadarSearchLayout(_: ReleaseRadarSearchLayoutProps) {
    return (
        <Box py={{ base: "md", md: "xl" }}>
            <ReleaseRadar />
        </Box>
    );
}
