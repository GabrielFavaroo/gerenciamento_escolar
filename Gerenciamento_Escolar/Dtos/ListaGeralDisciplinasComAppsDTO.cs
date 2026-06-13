namespace Gerenciamento_Escolar.Dtos;

public record ListaGeralDisciplinasComAppsDTO(
    int pagina,
    int quantidade,
    List<DisciplinaItemComAppsDTO> disciplinas
);