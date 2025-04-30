using JwtAspNet.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de TokenService ao contêiner de injeção de dependência
// Isso permite que o TokenService seja injetado em controladores ou outros serviços
builder.Services.AddTransient<TokenService>();

var app = builder.Build();

// Configura o pipeline de requisições HTTP
// Define um endpoint GET na raiz ("/") que cria e retorna um token JWT
app.MapGet("/", (TokenService tokenService) =>
{
    var token = tokenService.Create();
    return Results.Ok(token);
});

app.Run();
