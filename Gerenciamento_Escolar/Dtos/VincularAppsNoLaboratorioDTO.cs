using System.ComponentModel.DataAnnotations;

namespace Gerenciamento_Escolar.Dtos;

public record VincularAppsNoLaboratorioDTO([Required(AllowEmptyStrings = false)]string laboratorio,List<string> aplicativos);