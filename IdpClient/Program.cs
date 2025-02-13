var client = new HttpClient();

var tokenEndpoint = "https://localhost:7019/connect/token";

// request body.
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
