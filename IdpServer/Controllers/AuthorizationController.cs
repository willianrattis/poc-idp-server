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
            // The client credentials are automatically validated by OpenIddict.
            var application = await applicationManager.FindByClientIdAsync(request.ClientId) ??
                              throw new InvalidOperationException("The application cannot be found.");

            // Create a new ClaimsIdentity containing the claims used to create the token.
            var identity = new ClaimsIdentity(TokenValidationParameters.DefaultAuthenticationType,
                OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);

            // Use the client_id as the subject identifier.
            identity.SetClaim(OpenIddictConstants.Claims.Subject,
                await applicationManager.GetClientIdAsync(application));
            identity.SetClaim(OpenIddictConstants.Claims.Name,
                await applicationManager.GetDisplayNameAsync(application));

            // Set destinations for claims based on scopes.
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

        throw new NotImplementedException("The specified grant is not implemented.");
    }
}