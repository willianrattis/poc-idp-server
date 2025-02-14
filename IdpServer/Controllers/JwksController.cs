namespace IdpServer.Controllers;

[ApiController]
[Route(".well-known")]
public class JwksController(RsaSecurityKey signingKey) : ControllerBase
{
    [HttpGet("jwks")]
    public IActionResult GetJwks()
    {
        // Extrai os parâmetros públicos da chave RSA.
        RSAParameters rsaParameters = signingKey.Rsa.ExportParameters(false);

        // Converte os parâmetros para Base64URL conforme RFC 7517.
        var jwk = new
        {
            kty = "RSA",
            use = "sig",
            kid = signingKey.KeyId,
            alg = "RS256",
            n = Base64UrlEncoder.Encode(rsaParameters.Modulus),
            e = Base64UrlEncoder.Encode(rsaParameters.Exponent)
        };

        // Cria o conjunto de chaves (JWKS)
        var jwks = new { keys = new[] { jwk } };

        return Ok(jwks);
    }
}