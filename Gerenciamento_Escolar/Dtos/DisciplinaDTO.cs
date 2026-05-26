using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record DisciplinaDTO(
[Required(AllowEmptyStrings = false)]string nome,
[Range(1,int.MaxValue)]int alunos_matriculados,
[Required(AllowEmptyStrings = false)]string descricao,
[Range(1,int.MaxValue)]int duracao_meses,
TimeOnly horario_inicio_aula,
TimeOnly horario_fim_aula,
[Range(1,int.MaxValue)]int coordenador_id);