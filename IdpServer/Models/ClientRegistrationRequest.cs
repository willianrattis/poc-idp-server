namespace IdpServer.Models;

public class ClientRegistrationRequest
{
    /// <summary>
    /// Obtém ou define o identificador único do cliente.
    /// Esse valor deve ser único e é usado para identificar a aplicação durante os fluxos de autenticação/autorização.
    /// </summary>
    [Required]
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o segredo do cliente.
    /// Esse valor é utilizado para autenticar o cliente no fluxo de client credentials.
    /// </summary>
    [Required]
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o nome amigável da aplicação, utilizado para exibição.
    /// Esse valor será utilizado para definir a propriedade DisplayName do cliente.
    /// </summary>
    [Required]
    public string ClientName { get; set; } = string.Empty;

    // A seguir, estão listadas as propriedades adicionais do OpenIddictApplicationDescriptor,
    // que podem ser utilizadas para configurar de forma mais completa a aplicação.
    // Elas estão comentadas apenas para fins de documentação e elucidação.

    ///// <summary>
    ///// Indica o tipo da aplicação (por exemplo, "web", "native" ou "machine").
    ///// </summary>
    //public string? ApplicationType { get; set; }

    ///// <summary>
    ///// Define se a aplicação é "confidential" (pode manter o segredo de forma segura) ou "public" (não consegue manter segredos).
    ///// </summary>
    //public string? ClientType { get; set; }

    ///// <summary>
    ///// Especifica o tipo de consentimento exigido para a aplicação (por exemplo, "explicit" ou "implicit").
    ///// </summary>
    //public string? ConsentType { get; set; }

    ///// <summary>
    ///// Um nome amigável para exibição da aplicação.
    ///// </summary>
    //public string? DisplayName { get; set; }

    ///// <summary>
    ///// Permite definir nomes localizados para a aplicação, conforme a cultura.
    ///// </summary>
    //public Dictionary<CultureInfo, string>? DisplayNames { get; set; }

    ///// <summary>
    ///// Permite associar um conjunto de chaves (JWKS) à aplicação, para cenários avançados de criptografia ou assinatura.
    ///// </summary>
    //public JsonWebKeySet? JsonWebKeySet { get; set; }

    ///// <summary>
    ///// Conjunto de permissões que indicam os endpoints e fluxos que o cliente pode acessar.
    ///// Exemplo: ["ept:token", "gt:client_credentials"].
    ///// </summary>
    //public HashSet<string>? Permissions { get; set; }

    ///// <summary>
    ///// Conjunto de URIs para onde o usuário será redirecionado após o logout.
    ///// </summary>
    //public HashSet<Uri>? PostLogoutRedirectUris { get; set; }

    ///// <summary>
    ///// Conjunto de URIs de redirecionamento utilizados em fluxos interativos (como o Authorization Code Flow).
    ///// </summary>
    //public HashSet<Uri>? RedirectUris { get; set; }

    ///// <summary>
    ///// Conjunto de requisitos ou restrições que o cliente deve atender.
    ///// </summary>
    //public HashSet<string>? Requirements { get; set; }

    ///// <summary>
    ///// Propriedades adicionais personalizadas da aplicação.
    ///// </summary>
    //public Dictionary<string, JsonElement>? Properties { get; set; }

    ///// <summary>
    ///// Configurações adicionais que possam ser necessárias para a aplicação.
    ///// </summary>
    //public Dictionary<string, string>? Settings { get; set; }
}