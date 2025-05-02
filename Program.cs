using JwtAspNet.Models;
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
    var user = new User( 
        1, 
        "Silvio", 
        "silvio.sebastiany@gmail.com", 
        "https://avatars.githubusercontent.com/u/1021230?v=4",
        "123456", 
        new string[] { "Admin", "User" }
    ); 

    // Chama o método Create do TokenService para gerar um token JWT para o usuário
    return tokenService.Create(user);

});

app.Run();
