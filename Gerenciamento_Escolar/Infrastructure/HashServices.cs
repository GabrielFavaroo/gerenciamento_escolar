using System.Security.Cryptography;
using System.Text;
using Gerenciamento_Escolar.Infrastructure;
using Microsoft.Extensions.Options;

namespace Gerenciamento_Escolar.Services;

public class HashServices(IOptions<SecuritySettings> options)
{
    public string toHashPassword(string text)
    {
        var settings = options.Value;
        
        var encodedText = Encoding.ASCII.GetBytes(text);

        var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(settings.passwordSalt));
            
        var hashedText = hmac.ComputeHash(encodedText);

        return BitConverter.ToString(hashedText);
        
    }
}