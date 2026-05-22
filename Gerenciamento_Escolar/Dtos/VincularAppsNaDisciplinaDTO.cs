using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record VincularAppsNaDisciplinaDTO([Range(1,int.MaxValue)]int disciplinaId,List<int> idsDeAplicativos);