namespace Gerenciamento_Escolar.Dtos;

public record CredenciaisDeUsuarioDTO([Required(AllowEmptyStrings = false)]string nome,[Required(AllowEmptyStrings = false)] string senha);