import type { ReactNode } from "react";
import { Link } from "@tanstack/react-router";
import { Card } from "@astryxdesign/core/Card";
import { Text } from "@astryxdesign/core/Text";

import type { GameBrowseDto } from "@src/gen/catalogApi";
import { getIGDBImageUrl } from "@src/utils/ImageHelper";

type Column = {
    header: string;
    render: (game: GameBrowseDto) => ReactNode;
    numeric?: boolean;
};

type Props = {
    title: string;
    icon?: ReactNode;
    games: GameBrowseDto[];
    columns: Column[];
};

export function HomeTable({ title, icon, games, columns }: Props) {
    return (
        <Card padding={2}>
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
                <thead>
                    <tr>
                        <th colSpan={2} style={{ padding: "0", fontWeight: 400, fontSize: "1.125rem", textAlign: "left", borderBottom: "1px solid var(--border-color, var(--bg-subtle))", paddingBottom: "0.5rem" }}>
                            <div style={{ display: "inline-flex", alignItems: "center", gap: "0.5rem" }}>
                                {icon}
                                <Text>{title}</Text>
                            </div>
                        </th>
                        {columns.map((col) => (
                            <th
                                key={col.header}
                                style={{
                                    textAlign: col.numeric ? "right" : "center",
                                    borderBottom: "1px solid var(--border-color, var(--bg-subtle))",
                                    paddingBottom: "0.5rem",
                                    fontWeight: 400,
                                    fontSize: "0.875rem",
                                    color: "var(--fg-muted)",
                                }}
                            >
                                {col.header}
                            </th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {games.map((game) => (
                        <tr key={game.id} style={{ borderBottom: "1px solid var(--border-color, var(--bg-subtle))" }}>
                            <td style={{ padding: 0, verticalAlign: "middle", width: "1px" }}>
                                <Link
                                    to="/games/$gameId"
                                    params={{ gameId: String(game.id) }}
                                    style={{ display: "inline-flex", alignItems: "center", color: "inherit", textDecoration: "none" }}
                                >
                                    <img
                                        src={
                                            game.steam?.headerUrl ??
                                            getIGDBImageUrl(game.coverUrl, "cover_small")
                                        }
                                        style={{ height: "40px", display: "block", width: "7rem", objectFit: "cover", borderRadius: "var(--radius-sm)" }}
                                        alt={game.name ?? "Game cover"}
                                    />
                                </Link>
                            </td>
                            <td style={{ verticalAlign: "middle", padding: "0.5rem" }}>
                                <Link
                                    to="/games/$gameId"
                                    params={{ gameId: String(game.id) }}
                                    style={{
                                        color: "inherit",
                                        textDecoration: "none",
                                    }}
                                >
                                    <Text>{game.name}</Text>
                                </Link>
                            </td>
                            {columns.map((col) => (
                                <td
                                    key={col.header}
                                    style={{
                                        textAlign: col.numeric ? "right" : "center",
                                        verticalAlign: "middle",
                                        padding: "0.5rem",
                                    }}
                                >
                                    {col.render(game)}
                                </td>
                            ))}
                        </tr>
                    ))}
                </tbody>
            </table>

        </Card>
    );
}
