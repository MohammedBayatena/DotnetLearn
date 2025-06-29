namespace AuthLearn.Options;

public class JwtSettingsConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    
    public int RefreshTokenValidityDays { get; set; }
}