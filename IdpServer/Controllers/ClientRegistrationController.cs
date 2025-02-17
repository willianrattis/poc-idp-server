namespace IdpServer.Controllers;

[ApiController]
[Route("connect/register")]
public class ClientRegistrationController(IOpenIddictApplicationManager applicationManager) : ControllerBase
{
    /// <summary>
    /// Endpoint para registro dinâmico de clientes conforme RFC 7591.
    /// </summary>
    /// <param name="request">Dados do cliente a ser registrado.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Resultado da operação.</returns>
    [HttpPost, Produces("application/json")]
    public async Task<IActionResult> RegisterClient([FromBody] ClientRegistrationRequest request,
        CancellationToken cancellationToken)
    {
        // Validação simples dos dados recebidos.
        if (string.IsNullOrWhiteSpace(request.ClientId) || string.IsNullOrWhiteSpace(request.ClientSecret))
        {
            return BadRequest("ClientId and ClientSecret are required.");
        }

        // Verifica se o cliente já está registrado.
        if (await applicationManager.FindByClientIdAsync(request.ClientId, cancellationToken) is not null)
        {
            return Conflict("A client with the specified ClientId already exists.");
        }

        // Cria o descriptor do novo cliente.
        var descriptor = new OpenIddict.Abstractions.OpenIddictApplicationDescriptor
        {
            ClientId = request.ClientId,
            ClientSecret = request.ClientSecret,
            DisplayName = request.ClientName,
            Permissions =
            {
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.ClientCredentials
            }
        };

        // Cria o novo cliente.
        await applicationManager.CreateAsync(descriptor, cancellationToken);

        // Retorna uma resposta de sucesso.
        return CreatedAtAction(nameof(RegisterClient), new { clientId = request.ClientId },
            new { message = "Client registered successfully" });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetClients(CancellationToken cancellationToken)
    {
        // Cria uma lista para armazenar os dados dos clientes
        var clients = new List<ClientResponse>();

        // Itera sobre os clientes registrados
        await foreach (var client in applicationManager.ListAsync(cancellationToken: cancellationToken))
        {
            var clientId = await applicationManager.GetClientIdAsync(client, cancellationToken);
            var displayName = await applicationManager.GetDisplayNameAsync(client, cancellationToken);
            var clientType = await applicationManager.GetClientTypeAsync(client, cancellationToken);

            clients.Add(new ClientResponse
            {
                Id = clientId,
                DisplayName = displayName,
                ClientType = clientType
            });
        }

        return Ok(clients);
    }
}