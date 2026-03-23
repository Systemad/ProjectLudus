namespace PlayAPI.Data;

public class TypesenseKey
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}