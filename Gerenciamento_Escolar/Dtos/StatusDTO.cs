using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record StatusDTO([Required(AllowEmptyStrings = false)]string status);