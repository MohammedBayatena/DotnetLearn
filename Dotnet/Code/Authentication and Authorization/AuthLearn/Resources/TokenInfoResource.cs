namespace AuthLearn.Resources;

public class TokenInfoResource
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime Expires { get; set; }
}