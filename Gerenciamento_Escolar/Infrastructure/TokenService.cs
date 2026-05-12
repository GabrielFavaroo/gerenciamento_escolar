using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.IdentityModel.Tokens;

namespace Gerenciamento_Escolar.Infrastructure;

public class TokenService(JwtSecurityTokenHandler handler,Context context)
{
    public string receberToken(Usuario user)
    {

        var key = Encoding.ASCII.GetBytes(Configuration.encodingKey);

        var senhaInformada = hasherDeSenha(user.senha, key);

        if (!context.Usuarios.Any(u => u.nome == user.nome && u.senha == senhaInformada))
        {
            throw new Exception(message: "Usuario ou senha incorretos");
        }
        
        
        
        
     

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


    public string hasherDeSenha(string senha, byte[] key)
    {
        
        
        var senhaEncoded = Encoding.ASCII.GetBytes(senha);

        var hmac = new HMACSHA256(key);
            
           var senhaHashed = hmac.ComputeHash(senhaEncoded);

        return BitConverter.ToString(senhaHashed);

    }

    public static ClaimsIdentity GenerateClaims(Usuario user)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.nome));

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.tipoUsuario));
        return claimsIdentity;

    }


}