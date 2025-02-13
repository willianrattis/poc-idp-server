var builder = WebApplication.CreateBuilder(args);

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
        
        options.AllowClientCredentialsFlow();
        
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();
        
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

app.UseAuthentication();
app.UseAuthorization();
app.UseOpenApiConfig();
app.MapControllers();

app.Run();


