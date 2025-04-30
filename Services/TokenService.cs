using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JwtAspNet.Services;

public class TokenService
{
    public string Create()
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

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,           // Chave privada para assinar o token
            Expires = DateTime.UtcNow.AddHours(2),      // Tempo de expiração do token
        };


        var token = tokenHandler.CreateToken(tokenDescriptor); 
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
}