export type GameSearchHit = {
    id: string;
    name?: string;
    cover_url?: string;
    summary?: string;
    aggregated_rating?: number;
    aggregated_rating_count?: number;
    developers?: string[] | string;
    platforms?: string[] | string;
    game_type?: string;
    release_year?: number;
    first_release_date?: number | string;
};

export type CompanySearchHit = {
    id: string;
    name?: string;
    status?: string;
    games_developed_count?: number;
    games_published_count?: number;
    logo_url?: string;
};
