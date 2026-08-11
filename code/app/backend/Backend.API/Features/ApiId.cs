using System.Globalization;

namespace Backend.API.Features;

internal static class ApiId
{
    public static bool TryParse(string value, out long id) =>
        long.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out id) && id >= 0;

    public static long Parse(string value) =>
        long.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture);

    public static string Format(long id) => id.ToString(CultureInfo.InvariantCulture);
}
