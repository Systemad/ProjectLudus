export type CompanySearchHit = {
    id: string;
    name?: string;
    status?: string;
    games_developed_count?: number;
    games_published_count?: number;
    logo_url?: string;
};

export function getCompanyLogoUrl(logoUrl?: string) {
    if (!logoUrl) return "";
    if (logoUrl.startsWith("http")) return logoUrl;
    return `https://images.igdb.com/igdb/image/upload/t_logo_med/${logoUrl}.png`;
}

export function getCompanyStatusLabel(status?: string) {
    return status && status.trim().length > 0 ? status : "Unknown";
}
