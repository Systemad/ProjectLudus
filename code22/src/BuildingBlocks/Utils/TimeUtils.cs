using NodaTime;

namespace BuildingBlocks.Utils;

public static class TimeUtils
{
    public static Instant ToInstant(this DateTimeOffset? dateTimeOffset)
    {
        if (dateTimeOffset is not null)
        {
            return Instant.FromDateTimeOffset(dateTimeOffset.Value);
        }

        return Instant.FromUnixTimeTicks(0);
    }
}
