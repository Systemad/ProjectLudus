"use client";

import { Button } from "@astryxdesign/core/Button";
import { Grid } from "@astryxdesign/core/Grid";
import { Text } from "@astryxdesign/core/Text";
import { LuExternalLink } from "react-icons/lu";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import type { WebsiteDto } from "@src/gen/catalogApi";
type Props = {
    websites: WebsiteDto[];
};

export function OfficialLinks({ websites }: Props) {
    return (
        <div>
            <Text style={{marginBottom: "0.75rem"}}>Official Links</Text>
            {websites.length > 0 ? (
                <Grid columns={{minWidth: 280}} gap={3}>
                    {websites.map((website) => (
                        <Button
                            key={website.name}
                            label={website.type ?? "Official Link"}
                            as="a"
                            href={website.url ?? undefined}
                            target="_blank"
                            rel="noreferrer"
                            variant="ghost"
                            size="sm"
                            icon={<LuExternalLink />}
                            endContent={<PlatformIcon type={website.type!} tooltip={website.type!} />}
                        />
                    ))}
                </Grid>
            ) : (
                <Text style={{color: "var(--fg-tertiary)", fontSize: "0.875rem"}}>
                    No official links available.
                </Text>
            )}
        </div>
    );
}
