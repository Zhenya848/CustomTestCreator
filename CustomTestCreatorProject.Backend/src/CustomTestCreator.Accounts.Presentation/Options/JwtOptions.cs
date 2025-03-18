namespace CustomTestCreator.Accounts.Presentation.Options;

public class JwtOptions
{
    public const string JWT = "JWT";
    
    public string Audience { get; init; }
    public string Issuer { get; init; }
    public string Key { get; init; }
    public int ExpiredMinutesTime { get; init; }
}