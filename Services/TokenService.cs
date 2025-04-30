using System.IdentityModel.Tokens.Jwt;

namespace JwtAspNet.Services;

public class TokenService
{
    public string Create()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(); 
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
}