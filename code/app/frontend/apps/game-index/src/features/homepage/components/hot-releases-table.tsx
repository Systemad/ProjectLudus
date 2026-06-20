import type { GameBrowseDto } from "@src/gen/catalogApi";
import { Text } from "ui";

import { SteamIcon } from "@src/icons/Launchers/SteamIcon";
import { steamReviewColor, steamReviewRating } from "@src/utils/SteamReviewUtils";
import { PricingCell } from "./pricing-table-cell";
import { HomeTable } from "./home-table";

type Props = {
    games: GameBrowseDto[];
};

export function HotReleasesTable({ games }: Props) {
    return (
        <HomeTable
            title="Hot Releases"
            icon={<SteamIcon boxSize="1.25em" />}
            games={games}
            columns={[
                {
                    header: "Rating",
                    render: (g) => (
                        <Text color={steamReviewColor(g.review)}>
                            {steamReviewRating(g.review)}
                        </Text>
                    ),
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
