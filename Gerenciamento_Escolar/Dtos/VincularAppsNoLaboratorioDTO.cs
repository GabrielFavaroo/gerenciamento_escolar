using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record VincularAppsNoLaboratorioDTO([Range(1,int.MaxValue)]int laboratorioId,List<int> idsDeAplicativos);