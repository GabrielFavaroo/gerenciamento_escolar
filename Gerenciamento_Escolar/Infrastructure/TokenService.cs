using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Gerenciamento_Escolar.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gerenciamento_Escolar.Infrastructure;

public class TokenService(JwtSecurityTokenHandler handler,Context context, HashServices hashServices, IOptions<SecuritySettings> options)


{
    public Result<string> createToken(Usuario user)
    {
        var securitySettings = options.Value;
        
        var senhaInformada = hashServices.toHashPassword(user.senha);

        
        user = context.Usuarios.FirstOrDefault(u => u.nome == user.nome && u.senha == senhaInformada);

        if (user == null)
        {
            return Result<string>.Failure("Usuario não encontrado",404);}

        var keyBytes = Encoding.ASCII.GetBytes(securitySettings.jwtKey);

     var credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
        SecurityAlgorithms.HmacSha256Signature);

     
     
     
     var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = GenerateClaims(user),
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials = credentials,
        Issuer = "escola",
        Audience = "escola",
        
        
        
    };


    var token = handler.CreateToken(tokenDescriptor);

        return Result<string>.Success(handler.WriteToken(token),200);
    }
    
    


    

    public static ClaimsIdentity GenerateClaims(Usuario user)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.nome));

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.tipoUsuario));
        return claimsIdentity;

    }


}