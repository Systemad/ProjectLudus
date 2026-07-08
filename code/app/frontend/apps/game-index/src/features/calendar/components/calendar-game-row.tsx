import type { GameBrowseDto } from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";
import { PlatformIcon } from "@src/icons/PlatformIcon";
import { isTbaReleaseDate } from "@src/utils/dateUtils";
import { Link } from "@tanstack/react-router";
import { Text } from "@astryxdesign/core/Text";
import { HStack } from "@astryxdesign/core/HStack";
import { VStack } from "@astryxdesign/core/VStack";
import { Timestamp } from "@astryxdesign/core/Timestamp";

export function GameRow({
    game,
    isPlaceholder = false,
}: {
    game: GameBrowseDto;
    isPlaceholder?: boolean;
}) {
    const dayLabel = isPlaceholder ? (
        "Date TBA"
    ) : !isTbaReleaseDate(game.firstReleaseDate) && game.firstReleaseDate ? (
        <Timestamp value={new Date(game.firstReleaseDate).toISOString()} format="date" />
    ) : (
        "TBA"
    );
    const imageUrl = game.coverUrl ? getIGDBImageUrl(game.coverUrl, "cover_small") : null;
    const gameId = String(game.id);
    const studioLabel =
        game.companies.filter((c) => c.developer)[0]?.companyName ?? "Upcoming release";
    const platformIcons = game.platforms.slice(0, 4).map((platform) => platform.slug);

    return (
        <Link
            to="/games/$gameId"
            params={{ gameId }}
            style={{ display: "block", color: "inherit", textDecoration: "none" }}
        >
            <HStack vAlign="start" gap={3} style={{paddingLeft: "0.5rem", paddingRight: "0.5rem", paddingTop: "0.75rem", paddingBottom: "0.75rem", borderRadius: "var(--radius-md)", background: "var(--bg-surface)"}}>
                <div style={{ flexShrink: 0, width: "2.5rem", height: "3rem", borderRadius: "var(--radius-md)", overflow: "hidden", background: "var(--bg-subtle)" }}>
                    {imageUrl ? (
                        <img
                            src={imageUrl}
                            alt={game.name}
                            style={{ width: "100%", height: "100%", objectFit: "cover" }}
                            loading="lazy"
                        />
                    ) : (
                        <div style={{ display: "grid", placeItems: "center", width: "100%", height: "100%" }}>
                            <Text color="secondary" weight="semibold" style={{fontSize: "0.75rem"}}>
                                {game.name.slice(0, 1)}
                            </Text>
                        </div>
                    )}
                </div>

                <VStack hAlign="stretch" gap={1} style={{flex: 1, minWidth: 0}}>
                    <HStack hAlign="between" vAlign="start" gap={3} style={{minWidth: 0, width: "100%"}}>
                        <Text
                            weight="medium"
                            maxLines={2}
                            style={{fontSize: "0.875rem", minWidth: 0, flex: 1}}
                        >
                            {game.name}
                        </Text>
                        {dayLabel && (
                            <Text style={{fontSize: "0.75rem", flexShrink: 0, color: "var(--fg-tertiary)"}} justify="end">
                                {dayLabel}
                            </Text>
                        )}
                    </HStack>

                    <HStack hAlign="between" vAlign="center" gap={2} style={{minWidth: 0, width: "100%"}}>
                        <Text maxLines={1} style={{fontSize: "0.75rem", minWidth: 0, flex: 1, color: "var(--fg-tertiary)"}}>
                            {studioLabel}
                        </Text>
                        <HStack gap={2} vAlign="center" style={{flexShrink: 0}}>
                            {platformIcons.map((platform) => (
                                <PlatformIcon key={platform} type={platform} tooltip={platform} />
                            ))}
                        </HStack>
                    </HStack>
                </VStack>
            </HStack>
        </Link>
    );
}
