namespace SyncService.Seed;

public class SeedingJobHistory
{
    public int Id { get; set; }
    public SeedingType Type { get; set; }
    public SeedingResult Result { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Error { get; set; }
}

public enum SeedingType
{
    Initial,
    Catchup
}

public enum SeedingResult
{
    Success,
    Error
}