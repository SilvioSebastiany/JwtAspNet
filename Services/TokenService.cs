using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAspNet.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtAspNet.Services;

public class TokenService
{
    public string Create( User user)
    {
        // Cria um token JWT
        var tokenHandler = new JwtSecurityTokenHandler();

        // transforma a chave privada em bytes
        var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);


        // Cria as credenciais de assinatura do token
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        );

        // Cria os dados do token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,           // Chave privada para assinar o token
            Expires = DateTime.UtcNow.AddHours(2),      // Tempo de expiração do token
            Subject = GenerateClaims(user),             // Gera as claims (informações) do usuário

        };


        var token = tokenHandler.CreateToken(tokenDescriptor); 
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();

        // Adiciona as claims (informações) do usuário ao token
        // Essas claims podem ser usadas para identificar o usuário e suas permissões
        ci.AddClaim(new Claim("id", user.Id.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email)); 
        ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name)); 
        ci.AddClaim(new Claim("image", user.Image));

        foreach (var role in user.Roles)
        {
            ci.AddClaim(new Claim(ClaimTypes.Role, role)); // Adiciona cada papel (role) do usuário como uma claim
        }

        return ci;
    }
}