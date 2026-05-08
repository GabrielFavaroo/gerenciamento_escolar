using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record UsuarioDTO([Required(AllowEmptyStrings = false)]string nome,
    [Required(AllowEmptyStrings = false)]string email,
    [Required(AllowEmptyStrings = false)]string senha,
    [Required(AllowEmptyStrings = false)]string tipoUsuario);