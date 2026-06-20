import type { GameBrowseDto } from "@src/gen/catalogApi";
import { SteamIcon } from "@src/icons/Launchers/SteamIcon";
import { HomeTable } from "./home-table";

type Props = {
    games: GameBrowseDto[];
};

export function MostPlayedTable({ games }: Props) {
    return (
        <HomeTable
            title="Most Played"
            icon={<SteamIcon boxSize="1.25em" />}
            games={games}
            columns={[
                {
                    header: "Players Now",
                    numeric: true,
                    render: (g) => g.steam?.currentPlayers?.toLocaleString() ?? "—",
                },
                {
                    header: "24H Peak",
                    numeric: true,
                    render: (g) => g.steam?.peak24h?.toLocaleString() ?? "—",
                },
            ]}
        />
    );
}
