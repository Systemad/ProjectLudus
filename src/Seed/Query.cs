namespace Seed;

public static class Query
{
    public const string BaseUrl = "https://api.igdb.com/v4/";
    public const string CountSuffix = "/count";

    public static readonly Dictionary<string, string> DataConfig = new()
    {
        { Endpoints.AgeRatingOrganization, "checksum,created_at,name,updated_at;" },
        { Endpoints.AgeRatingCategories, "checksum,created_at,organization,rating,updated_at;" },
        { Endpoints.AgeRatingContentDescriptionsV2, "checksum,created_at,description,organization,updated_at;" },
        /*
        {
            Endpoints.AgeRatings,
            "category,checksum,content_descriptions,organization,rating,rating_category,rating_content_descriptions,rating_cover_url,synopsis;"
        },
        */
        { Endpoints.CharacterGenders, "checksum,created_at,name,updated_at;" },
        { Endpoints.CharacterSpecies, "checksum,created_at,name,updated_at;" },
        { Endpoints.CollectionTypes, "checksum,created_at,description,name,updated_at;" },
        { Endpoints.CompanyStatuses, "checksum,created_at,name,updated_at;" },
        { Endpoints.ExternalGameSources, "checksum,created_at,name,updated_at;" },
        { Endpoints.GameEngineLogos, "alpha_channel,animated,checksum,height,image_id,url,width;" },
        { Endpoints.GameModes, "checksum,created_at,name,slug,updated_at,url;" },
        { Endpoints.GameReleaseFormats, "checksum,created_at,format,updated_at;" },
        { Endpoints.GameStatuses, "checksum,created_at,status,updated_at;" },
        { Endpoints.GameTypes, "checksum,created_at,type,updated_at;" },
        { Endpoints.Genres, "checksum,created_at,name,slug,updated_at,url;" },
        { Endpoints.Languages, "checksum,created_at,locale,name,native_name,updated_at;" },
        { Endpoints.PlatformTypes, "checksum,created_at,name,updated_at;" },
        { Endpoints.PlatformWebsites, "category,checksum,trusted,url;" },
        { Endpoints.PlayerPerspectives, "checksum,created_at,name,slug,updated_at,url;" },
        { Endpoints.Regions, "category,checksum,created_at,identifier,name,updated_at;" },
        { Endpoints.ReleaseDateRegions, "checksum,created_at,region,updated_at;" },
        { Endpoints.ReleaseDateStatuses, "checksum,created_at,description,name,updated_at;" },
        { Endpoints.Themes, "checksum,created_at,name,slug,updated_at,url;" },
        { Endpoints.DateFormats, "checksum,created_at,format,updated_at;" },
        { Endpoints.WebsiteTypes, "checksum,created_at,type,updated_at;" },
        { Endpoints.LanguageSupportTypes, "checksum,created_at,name,updated_at;" },
        { Endpoints.PlatformLogos, "alpha_channel,animated,checksum,height,image_id,url,width;" },
        { Endpoints.PlatformFamilies, "checksum,name,slug;" }


        // TODO
        //{ "platform_versions", "checksum,companies,connectivity,cpu,graphics,main_manufacturer,media,memory,name,os,output,platform_logo,platform_version_release_dates,resolutions,slug,sound,storage,summary,url;" },

        //{ "platforms", "abbreviation,alternative_name,category,checksum,created_at,generation,name,platform_family,platform_logo,platform_type,slug,summary,updated_at,url,versions,websites; " },

        //{ "game_engines", "checksum,created_at,description,logo,name,platforms,slug,updated_at,url;" },

        // TODO
    };
}