using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Microsoft.IdentityModel.Tokens;

namespace Gerenciamento_Escolar.Infrastructure;

public class TokenService(JwtSecurityTokenHandler handler)
{
    private string login(Usuario user)
    {
        
     var key = Encoding.ASCII.GetBytes(Configuration.encodingKey);

     var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha256Signature);

     var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = GenerateClaims(user),
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials = credentials,
    };


    var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    


    private static ClaimsIdentity GenerateClaims(Usuario user)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.nome));

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.tipoUsuario));
        return claimsIdentity;

    }


}