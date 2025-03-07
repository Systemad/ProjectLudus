namespace Seed;



public static class Query
{
    public const string BaseUrl = "https://api.igdb.com/v4/";
    public const string CountSuffix = "/count";
    
    public static readonly Dictionary<string, string> DataConfig = new()
    {
        { "age_rating_organizations", "fields checksum,created_at,name,updated_at;" },
        { "age_rating_categories", "checksum,created_at,organization,rating,updated_at;" },
        { "age_rating_content_descriptions_v2", "fields checksum,created_at,description,organization,updated_at;" },
        { "age_ratings", "category,checksum,content_descriptions,organization,rating,rating_category,rating_content_descriptions,rating_cover_url,synopsis;" },
        { "character_genders", "checksum,created_at,name,updated_at;" },
        { "character_species", "checksum,created_at,name,updated_at;" },
        { "collection_types", "checksum,created_at,description,name,updated_at;" },
        { "company_statuses", "checksum,created_at,name,updated_at;" },
        { "external_game_sources", "checksum,created_at,name,updated_at;" },
        { "game_engine_logos", "alpha_channel,animated,checksum,height,image_id,url,width;" },
        { "game_modes", "checksum,created_at,name,slug,updated_at,url;" },
        { "game_release_formats", "checksum,created_at,format,updated_at;" },
        { "game_statuses", "checksum,created_at,status,updated_at;" },
        { "game_types", "checksum,created_at,type,updated_at;" },
        { "genres", "checksum,created_at,name,slug,updated_at,url;" },
        { "languages", "checksum,created_at,locale,name,native_name,updated_at;" },
        { "platform_types", "checksum,created_at,name,updated_at;" },
        { "platform_websites", "category,checksum,trusted,url;" },
        { "player_perspectives", "checksum,created_at,name,slug,updated_at,url;" },
        { "regions", "category,checksum,created_at,identifier,name,updated_at;" },
        { "release_date_regions", "checksum,created_at,region,updated_at;" },
        { "release_date_statuses", "checksum,created_at,description,name,updated_at;" },
        { "themes", "checksum,created_at,name,slug,updated_at,url;" },
        { "date_formats", "checksum,created_at,format,updated_at;" },
        { "website_types", "checksum,created_at,type,updated_at;" },
        { "language_support_types", "checksum,created_at,name,updated_at;" },
        { "platform_logos", "alpha_channel,animated,checksum,height,image_id,url,width;" },
        { "platform_families", "checksum,name,slug;" },

        
        // TODO
        //{ "platform_versions", "checksum,companies,connectivity,cpu,graphics,main_manufacturer,media,memory,name,os,output,platform_logo,platform_version_release_dates,resolutions,slug,sound,storage,summary,url;" },

        //{ "platforms", "abbreviation,alternative_name,category,checksum,created_at,generation,name,platform_family,platform_logo,platform_type,slug,summary,updated_at,url,versions,websites; " },

        //{ "game_engines", "checksum,created_at,description,logo,name,platforms,slug,updated_at,url;" },

        // TODO
    };
}

