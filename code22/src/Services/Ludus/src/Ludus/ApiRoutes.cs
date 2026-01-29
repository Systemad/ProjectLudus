namespace Ludus;

public static class ApiRoutes
{
    public const string Me = "api/me";

    public static class MeRoutes
    {
        public const string Profile = $"{Me}/profile";
        public const string Lists = $"{Me}/lists";
        public const string Favorites = $"{Me}/favorites";
        public const string Wishlist = $"{Me}/wishlist";
        public const string Hypes = $"{Me}/hypes";
        public const string GameMetadata = $"{Me}/metadata";
    }
}
