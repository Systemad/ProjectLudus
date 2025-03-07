namespace Seed;

public class TypeExtension
{
    public T GetDataType(string endpoint)
    {
        return endpoint switch
        {
            "age_rating_organizations" => typeof(AgeRatingOrganization),
            "age_rating_categories" => typeof(AgeRatingCategory),
            "age_rating_content_descriptions_v2" => typeof(AgeRatingContentDescription),
            "age_ratings" => typeof(AgeRating),
            "character_genders" => typeof(CharacterGender),
            "character_species" => typeof(CharacterSpecies),
            "collection_types" => typeof(CollectionType),
            "company_statuses" => typeof(CompanyStatus),
            "external_game_sources" => typeof(ExternalGameSource),
            "game_engine_logos" => typeof(GameEngineLogo),
            "game_modes" => typeof(GameMode),
            "game_release_formats" => typeof(GameReleaseFormat),
            "game_statuses" => typeof(GameStatus),
            "game_types" => typeof(GameType),
            "genres" => typeof(Genre),
            "languages" => typeof(Language),
            "platform_types" => typeof(PlatformType),
            "platform_websites" => typeof(PlatformWebsite),
            "player_perspectives" => typeof(PlayerPerspective),
            "regions" => typeof(Region),
            "release_date_regions" => typeof(ReleaseDateRegion),
            "release_date_statuses" => typeof(ReleaseDateStatus),
            "themes" => typeof(Theme),
            "date_formats" => typeof(DateFormat),
            "website_types" => typeof(WebsiteType),
            "language_support_types" => typeof(LanguageSupportType),
            "platform_logos" => typeof(PlatformLogo),
            "platform_families" => typeof(PlatformFamily),
            _ => throw new InvalidOperationException($"Unknown endpoint: {endpoint}")
        };
    }
}