using System.ComponentModel.DataAnnotations;
using Gerenciamento_Escolar.Models;

namespace Gerenciamento_Escolar.Dtos;

public record ListaDeLaboratorioDTO([Range(1, int.MaxValue)]int pagina,
    [Range(1, int.MaxValue)]int quantidade,
    List<Laboratorio> elementos);