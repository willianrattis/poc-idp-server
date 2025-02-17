var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

// Criar uma instância de RSA e importar a chave privada a partir do PEM.
RSA rsa = RSA.Create();
rsa.ImportFromPem(jwtOptions?.PrivateKey.ToCharArray());

// Criar o RsaSecurityKey usando a chave privada e definir o KeyId.
var signingKey = new RsaSecurityKey(rsa) { KeyId = jwtOptions?.KeyId };

// Registre a chave no DI para que possa ser injetada posteriormente.
builder.Services.AddSingleton<RsaSecurityKey>(signingKey);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseOpenIddict();
});


builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("connect/token");

        // Permitir o fluxo de client credentials.
        options.AllowClientCredentialsFlow();

        // Utilize sua chave para assinatura:
        options.AddSigningCredentials(new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256));

        // Registre uma chave de encriptação (opção 1 ou 2):
        options.AddDevelopmentEncryptionCertificate();
        // ou
        // options.AddEphemeralEncryptionKey();
        options.DisableAccessTokenEncryption();

        options.UseAspNetCore()
            .EnableTokenEndpointPassthrough();
    });

builder.Services.AddControllers();
builder.Services.AddHostedService<Worker>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiConfig();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.UseOpenApiConfig();
app.MapControllers();

app.Run();


