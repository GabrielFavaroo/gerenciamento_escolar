namespace Gerenciamento_Escolar.Infrastructure;

public sealed class SecuritySettings
{
    public string passwordSalt { get; set; }
    
    public string jwtKey { get; set; }
}