using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Models;

namespace Gerenciamento_Escolar.Dtos;

public record AlocacaoDTO(
    [Required(AllowEmptyStrings = false)] string titulo,
    [Required(AllowEmptyStrings = false)] string disciplina_nome,
    [Required(AllowEmptyStrings = false)] string laboratorio_nome,
    [Required(AllowEmptyStrings = false)] string dia_da_semana,
    [DataPresente]
    [Required]
    DateOnly dataAgendamento,
    TimeOnly horario_inicio,
    TimeOnly horario_fim,
    [Required(AllowEmptyStrings = false)] string coordenador_nome);
