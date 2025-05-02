using System.Text;
using JwtAspNet;
using JwtAspNet.Models;
using JwtAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de TokenService ao contêiner de injeção de dependência
// Isso permite que o TokenService seja injetado em controladores ou outros serviços
builder.Services.AddTransient<TokenService>(); 
// Adiciona a configuração de autenticação JWT ao contêiner de injeção de dependência
builder.Services.AddAuthentication(x => 
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de autenticação padrão como JWT Bearer
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema de desafio padrão como JWT Bearer
}).AddJwtBearer(x => 
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.PrivateKey)), // Chave privada para validação
        ValidateIssuer = false, // Não valida o emissor do token (pode ser ajustado conforme necessário)
        ValidateAudience = false, // Não valida o público do token (pode ser ajustado conforme necessário)
    };
});  
builder.Services.AddAuthorization();  


var app = builder.Build();

// Habilita a autenticação no pipeline de requisições HTTP
// Isso permite que o ASP.NET Core valide os tokens JWT em requisições recebidas
app.UseAuthentication(); 
app.UseAuthorization();   

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
