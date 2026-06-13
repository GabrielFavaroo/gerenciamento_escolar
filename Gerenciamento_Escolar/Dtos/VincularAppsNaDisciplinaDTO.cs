using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record VincularAppsNaDisciplinaDTO([Required(AllowEmptyStrings = false)]string disciplina,List<string> aplicativos);