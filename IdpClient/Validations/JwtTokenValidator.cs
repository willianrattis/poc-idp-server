namespace IdpClient.Validations;

public class JwtTokenValidator
{
    private readonly string _expectedAudience;
    
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;

    public JwtTokenValidator(string discoveryEndpoint, string expectedAudience)
    {
        var discoveryEndpoint1 = discoveryEndpoint ?? throw new ArgumentNullException(nameof(discoveryEndpoint));
        _expectedAudience = expectedAudience ?? throw new ArgumentNullException(nameof(expectedAudience));

        var documentRetriever = new HttpDocumentRetriever { RequireHttps = true };
        _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            discoveryEndpoint1,
            new OpenIdConnectConfigurationRetriever(),
            documentRetriever);
    }

    public async Task<ClaimsPrincipal> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        // Obtenha a configuração de OpenID Connect (incluindo o issuer e as chaves de assinatura).
        var openIdConfig = await _configurationManager.GetConfigurationAsync(cancellationToken);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = openIdConfig.Issuer,
            
            ValidateAudience = true,
            ValidAudience = _expectedAudience,
            
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = openIdConfig.SigningKeys,
            
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken validatedToken;
        return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
    }
}