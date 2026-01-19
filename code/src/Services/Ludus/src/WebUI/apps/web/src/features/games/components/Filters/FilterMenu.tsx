import { Accordion, memo } from "@yamada-ui/react";
import { FilterAccordion } from "../Accordion/FilterAccordionItem";

type FiltersPanelProps = {
    filters: {
        platforms: { id: number; name: string }[];
        genres: { id: number; name: string }[];
    };
    selectedPlatforms: number[];
    selectedGenres: number[];
    onPlatformsChange: (ids: number[]) => void;
    onGenresChange: (ids: number[]) => void;
};

export const FiltersPanel = memo(
    ({
        filters,
        selectedPlatforms,
        selectedGenres,
        onPlatformsChange,
        onGenresChange,
    }: FiltersPanelProps) => {
        return (
            <Accordion defaultIndex={[0, 1]} multiple variant="card">
                <FilterAccordion
                    title="Platforms"
                    items={filters.platforms}
                    selected={selectedPlatforms}
                    onChange={onPlatformsChange}
                />
                <FilterAccordion
                    title="Genres"
                    items={filters.genres}
                    selected={selectedGenres}
                    onChange={onGenresChange}
                />
            </Accordion>
        );
    }
);
