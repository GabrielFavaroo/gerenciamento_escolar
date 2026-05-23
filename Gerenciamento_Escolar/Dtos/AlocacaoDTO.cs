using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Models;

namespace Gerenciamento_Escolar.Dtos;

public record AlocacaoDTO(
    [Range(1, int.MaxValue)] int disciplina_id,
    [Range(1, int.MaxValue)] int laboratorio_id,
    string dia_da_semana,
    [DataPresente]
    [Required]
    DateOnly dataAgendamento,
    TimeSpan horario_inicio,
    TimeSpan horario_fim,
    [Range(1, int.MaxValue)] int coordenadorId);
