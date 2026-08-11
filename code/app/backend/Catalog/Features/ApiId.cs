using System.Globalization;

namespace Catalog.Features;

internal static class ApiId
{
    public static bool TryParse(string value, out long id) =>
        long.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out id) && id >= 0;

    public static string Format(long id) => id.ToString(CultureInfo.InvariantCulture);

    public static string? Format(long? id) => id?.ToString(CultureInfo.InvariantCulture);
}
