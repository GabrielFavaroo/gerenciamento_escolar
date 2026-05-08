using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record LaboratorioDTO([Required(AllowEmptyStrings = false)]string nome,
    [Range(1,int.MaxValue)] int qnt_computadores);