using System.Security.Claims;
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
builder.Services.AddAuthorization(x => 
{
    // Define uma política de autorização chamada "admin" que requer o papel "Admin"
    x.AddPolicy("admin", b => b.RequireRole("Admin")); 
}); 


var app = builder.Build();

// Habilita a autenticação no pipeline de requisições HTTP
// Isso permite que o ASP.NET Core valide os tokens JWT em requisições recebidas
app.UseAuthentication(); 
app.UseAuthorization();   

// Configura o pipeline de requisições HTTP
// Define o endpoint de login que gera um token JWT para o usuário
// O endpoint é acessível sem autenticação, pois é onde o token é gerado
app.MapGet("/login", (TokenService tokenService) =>
{
    var user = new User( 
        1, 
        "Silvio", 
        "silvio.sebastiany@gmail.com", 
        "https://avatars.githubusercontent.com/u/1021230?v=4",
        "123456", 
        new string[] { "User" }
    ); 

    // Chama o método Create do TokenService para gerar um token JWT para o usuário
    return tokenService.Create(user);

});

app.MapGet("/restrito",(ClaimsPrincipal user) => new
{
    id = user.Claims.FirstOrDefault(x => x.Type == "id")?.Value,
    name = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
    email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
    giveName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value,
    image = user.Claims.FirstOrDefault(x => x.Type == "image")?.Value, 
}).RequireAuthorization();

app.MapGet("/admin",() => "You tem acesso")
    .RequireAuthorization("admin"); // Define um endpoint protegido que requer autenticação e o papel "admin"


app.Run();
