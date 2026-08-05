import type { GameBrowseDto } from "@src/gen/catalogApi";
import { SteamIcon } from "@src/icons/Launchers/SteamIcon";
import { PricingCell } from "./pricing-table-cell";
import { HomeTable } from "./home-table";

type Props = {
    games: GameBrowseDto[];
};

export function PopularReleasesTable({ games }: Props) {
    return (
        <HomeTable
            title="Popular Releases"
            icon={<SteamIcon boxSize="1.25em" />}
            games={games}
            columns={[
                {
                    header: "24H Peak",
                    numeric: true,
                    render: (g) => g.steam?.peak24h?.toLocaleString() ?? "-",
                },
                {
                    header: "Price",
                    numeric: true,
                    render: (g) => <PricingCell pricing={g.pricing} />,
                },
            ]}
        />
    );
}
