namespace CatalogAPI.Common;

public static class ApiRoutes
{
    public const string Games = "api/games";

    public static class GameRoutes
    {
        public const string Popular = $"{Games}/profile";
        public const string Upcoming = $"{Games}/favorites";
        public const string Released = $"{Games}/wishlist";
        public const string Calendar = $"{Games}/hypes";
    }
}
