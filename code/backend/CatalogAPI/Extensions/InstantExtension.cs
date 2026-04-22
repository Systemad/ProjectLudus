namespace CatalogAPI.Extensions;

public static class InstantExtensions
{
    public static DateOnly? ToDateOnly(this Instant? instant) => 
        instant?.ToDateTimeUtc() is { } utc ? DateOnly.FromDateTime(utc) : null;
}