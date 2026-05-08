using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record AlocacaoAtualizadaDTO(
    [Range(1, int.MaxValue)] int disciplina_id,
    [Range(1, int.MaxValue)] int laboratorio_id,
    string dia_da_semana,
    TimeSpan horario_inicio,
    TimeSpan horario_fim,
    string status,
        int aprovadoPorId,
    DateTime dataAprovacao,
    int coordenadorId
        );
