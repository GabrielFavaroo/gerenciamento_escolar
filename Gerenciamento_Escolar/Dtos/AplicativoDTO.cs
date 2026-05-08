using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record AplicativoDTO([Required(AllowEmptyStrings = false)]string nome,
    [Required(AllowEmptyStrings = false)]string versao,
    [Required(AllowEmptyStrings = false)]string descricao);