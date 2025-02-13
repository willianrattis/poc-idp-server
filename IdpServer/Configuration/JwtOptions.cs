namespace IdpServer.Configuration;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int TokenLifetimeInMinutes { get; set; }
    public string PrivateKey { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
}
