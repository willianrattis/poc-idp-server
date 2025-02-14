namespace IdpClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        var client = new HttpClient();

        var tokenEndpoint = "https://localhost:7019/connect/token";

        // Corpo da requisição
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", "service-worker" },
            { "client_secret", "388D45FA-B36B-4988-BA59-B187D329C207" }
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await client.PostAsync(tokenEndpoint, content);
        var jsonResponse = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Token response:");
        Console.WriteLine(jsonResponse);

        // Desserializar a resposta e extrair o access_token.
        using var document = JsonDocument.Parse(jsonResponse);
        if (!document.RootElement.TryGetProperty("access_token", out var tokenElement))
        {
            Console.WriteLine("Token não encontrado na resposta.");
            return;
        }
        string token = tokenElement.GetString()!;

        // Configurar o endpoint de discovery. Geralmente ele fica em:
        // https://localhost:7019/.well-known/openid-configuration
        string discoveryEndpoint = "https://localhost:7019/.well-known/openid-configuration";
        string expectedAudience = "https://casashabia.com.br";

        var validator = new JwtTokenValidator(discoveryEndpoint, expectedAudience);

        try
        {
            var principal = await validator.ValidateTokenAsync(token);
            Console.WriteLine("Token válido!");
            foreach (var claim in principal.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro na validação do token: " + ex.Message);
        }
    }
}