using Shared;

namespace Seed;

public static class Hey
{
    public static Dictionary<string, IResponseBase> MapTypes = new Dictionary<string, IResponseBase>();
}

public class SeedService
{
    private readonly Fetch _fetch;

    public SeedService()
    {
        _fetch = new Fetch();
    }

    public async Task FetchAndInsertDataAsync()
    {
        using var db = new SeedContext();
        var tasks = new List<Task>();

        foreach (var entry in Query.DataConfig)
        {
            var endpoint = entry.Key;
            var fields = entry.Value;

            Task fetchtask;
            switch (endpoint)
            {
                case Endpoints.AgeRatingOrganization:
                    var ageData =
                        await _fetch.FetchAllDataAsync<AgeRatingOrganizations>(endpoint, fields);
                    db.AgeRatingOrganizations.AddRange(ageData);
                    break;
                case Endpoints.AgeRatingCategories:
                    var ageRatingCategories =
                        await _fetch.FetchAllDataAsync<AgeRatingCategories>(endpoint, fields);
                    db.AgeRatingCategories.AddRange(ageRatingCategories);
                    break;
                case Endpoints.AgeRatingContentDescriptionsV2:
                    var ageRatingContent =
                        await _fetch.FetchAllDataAsync<AgeRatingContentDescriptionsV2>(endpoint, fields);
                    db.AgeRatingContentDescriptionsV2.AddRange(ageRatingContent);
                    break;
                /*
                case Endpoints.AgeRatings:
                    var ageRatings = await _fetch.FetchAllDataAsync<AgeRatings>(endpoint, fields);
                    db.AgeRatings.AddRange(ageRatings);
                    break;
                    */
                case Endpoints.CharacterGenders:
                    var characterGenders =
                        await _fetch.FetchAllDataAsync<CharacterGenders>(endpoint, fields);
                    db.CharacterGenders.AddRange(characterGenders);
                    break;
                case Endpoints.CharacterSpecies:
                    var characterSpecies =
                        await _fetch.FetchAllDataAsync<CharacterSpecies>(endpoint, fields);
                    db.CharacterSpecies.AddRange(characterSpecies);
                    break;
                case Endpoints.CollectionTypes:
                    var collectionTypes =
                        await _fetch.FetchAllDataAsync<CollectionTypes>(endpoint, fields);
                    db.CollectionTypes.AddRange(collectionTypes);
                    break;
                case Endpoints.CompanyStatuses:
                    var companyStatuses =
                        await _fetch.FetchAllDataAsync<CompanyStatuses>(endpoint, fields);
                    db.CompanyStatuses.AddRange(companyStatuses);
                    break;
                case Endpoints.ExternalGameSources:
                    var externalGameSources =
                        await _fetch.FetchAllDataAsync<ExternalGameSources>(endpoint, fields);
                    db.ExternalGameSources.AddRange(externalGameSources);
                    break;
                case Endpoints.GameEngineLogos:
                    var gameEngineLogos =
                        await _fetch.FetchAllDataAsync<GameEngineLogos>(endpoint, fields);
                    db.GameEngineLogos.AddRange(gameEngineLogos);
                    break;
                case Endpoints.GameModes:
                    var gameModes = await _fetch.FetchAllDataAsync<GameModes>(endpoint, fields);
                    db.GameModes.AddRange(gameModes);
                    break;
                case Endpoints.GameReleaseFormats:
                    var gameReleaseFormats =
                        await _fetch.FetchAllDataAsync<GameReleaseFormats>(endpoint, fields);
                    db.GameReleaseFormats.AddRange(gameReleaseFormats);
                    break;
                case Endpoints.GameStatuses:
                    var gameStatuses =
                        await _fetch.FetchAllDataAsync<GameStatuses>(endpoint, fields);
                    db.GameStatuses.AddRange(gameStatuses);
                    break;
                case Endpoints.GameTypes:
                    var gameTypes = await _fetch.FetchAllDataAsync<GameTypes>(endpoint, fields);
                    db.GameTypes.AddRange(gameTypes);
                    break;
                case Endpoints.Genres:
                    var genres = await _fetch.FetchAllDataAsync<Genres>(endpoint, fields);
                    db.Genres.AddRange(genres);
                    break;
                case Endpoints.Languages:
                    var languages = await _fetch.FetchAllDataAsync<Languages>(endpoint, fields);
                    db.Languages.AddRange(languages);
                    break;
                case Endpoints.PlatformTypes:
                    var platformTypes =
                        await _fetch.FetchAllDataAsync<PlatformTypes>(endpoint, fields);
                    db.PlatformTypes.AddRange(platformTypes);
                    break;
                case Endpoints.PlatformWebsites:
                    var platformWebsites =
                        await _fetch.FetchAllDataAsync<PlatformWebsites>(endpoint, fields);
                    db.PlatformWebsites.AddRange(platformWebsites);
                    break;
                case Endpoints.PlayerPerspectives:
                    var playerPerspectives =
                        await _fetch.FetchAllDataAsync<PlayerPerspectives>(endpoint, fields);
                    db.PlayerPerspectives.AddRange(playerPerspectives);
                    break;
                case Endpoints.Regions:
                    var regions = await _fetch.FetchAllDataAsync<Regions>(endpoint, fields);
                    db.Regions.AddRange(regions);
                    break;
                case Endpoints.ReleaseDateRegions:
                    var releaseDateRegions =
                        await _fetch.FetchAllDataAsync<ReleaseDateRegions>(endpoint, fields);
                    db.ReleaseDateRegions.AddRange(releaseDateRegions);
                    break;
                case Endpoints.ReleaseDateStatuses:
                    var releaseDateStatuses =
                        await _fetch.FetchAllDataAsync<ReleaseDateStatuses>(endpoint, fields);
                    db.ReleaseDateStatuses.AddRange(releaseDateStatuses);
                    break;
                case Endpoints.Themes:
                    var themes = await _fetch.FetchAllDataAsync<Themes>(endpoint, fields);
                    db.Themes.AddRange(themes);
                    break;
                case Endpoints.DateFormats:
                    var dateFormats = await _fetch.FetchAllDataAsync<DateFormats>(endpoint, fields);
                    db.DateFormats.AddRange(dateFormats);
                    break;
                case Endpoints.WebsiteTypes:
                    var websiteTypes =
                        await _fetch.FetchAllDataAsync<WebsiteTypes>(endpoint, fields);
                    db.WebsiteTypes.AddRange(websiteTypes);
                    break;
                case Endpoints.LanguageSupportTypes:
                    var languageSupportTypes =
                        await _fetch.FetchAllDataAsync<LanguageSupportTypes>(endpoint, fields);
                    db.LanguageSupportTypes.AddRange(languageSupportTypes);
                    break;
                case Endpoints.PlatformLogos:
                    var platformLogos =
                        await _fetch.FetchAllDataAsync<PlatformLogos>(endpoint, fields);
                    db.PlatformLogos.AddRange(platformLogos);
                    break;
                case Endpoints.PlatformFamilies:
                    var platformFamilies =
                        await _fetch.FetchAllDataAsync<PlatformFamilies>(endpoint, fields);
                    db.PlatformFamilies.AddRange(platformFamilies);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await db.SaveChangesAsync();
            //tasks.Add(fetchtask);
        }

        // Execute all tasks concurrently
       // await Task.WhenAll(tasks);
    }
}