using System.Security.Cryptography;
using System.Text;
using Gerenciamento_Escolar.Infrastructure;

namespace Gerenciamento_Escolar.Services;

public class HashServices(SecuritySettings settings)
{
    public string toHashPassword(string text)
    {
        
        var encodedText = Encoding.ASCII.GetBytes(text);

        var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(settings.passwordSalt));
            
        var hashedText = hmac.ComputeHash(encodedText);

        return BitConverter.ToString(hashedText);
        
    }
}