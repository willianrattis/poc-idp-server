namespace IdpServer.Models;

public class ClientResponse
{
    /// <summary>
    /// Obtém ou define o identificador único do cliente.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Obtém ou define o nome amigável da aplicação, utilizado para exibição.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Obtém ou define o tipo do cliente, como "confidential" ou "public".
    /// </summary>
    public string? ClientType { get; set; }
    
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