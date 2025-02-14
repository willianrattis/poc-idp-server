namespace IdpServer.Controllers;

[ApiController]
public class AuthorizationController(IOpenIddictApplicationManager applicationManager) : ControllerBase
{
    [HttpPost("~/connect/token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        if (request.IsClientCredentialsGrantType())
        {
            // As credenciais do cliente são validadas automaticamente pelo OpenIddict.
            var application = await applicationManager.FindByClientIdAsync(request.ClientId) ??
                              throw new InvalidOperationException("O aplicativo não pode ser encontrado.");

            // Crie um novo ClaimsIdentity contendo as declarações usadas para criar o token.
            var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType,
                OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);

            // Use o client_id como identificador do assunto.
            identity.SetClaim(OpenIddictConstants.Claims.Subject,
                await applicationManager.GetClientIdAsync(application));
            identity.SetClaim(OpenIddictConstants.Claims.Name,
                await applicationManager.GetDisplayNameAsync(application));
            
            // Adicione um público (claim "aud")
            identity.SetClaim("aud", "https://casashabia.com.br");

            // Defina destinos para declarações com base em escopos.
            identity.SetDestinations(claim => claim.Type switch
            {
                OpenIddictConstants.Claims.Name when claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes
                        .Profile)
                    => new[]
                    {
                        OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken
                    },
                _ => new[] { OpenIddictConstants.Destinations.AccessToken }
            });

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new NotImplementedException("A concessão especificada não foi implementada.");
    }
}