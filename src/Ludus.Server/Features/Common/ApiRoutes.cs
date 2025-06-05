namespace Ludus.Server.Features.Common;

public static class ApiRoutes
{
    public const string Games = "api/games";
    public const string Users = "api/users";
    public const string Collections = "api/collection";
    public const string Me = "api/me";

    public static class MeRoutes
    {
        public const string Profile = $"{Me}/profile";
        public const string Collections = $"{Me}/collections";
        public const string Lists = $"{Me}/lists";
    }
}
