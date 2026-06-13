namespace Gerenciamento_Escolar.Dtos;

public record DisciplinaItemComAppsDTO(
    int id,
    string nome,
    string descricao,
    List<AplicativoVinculadoDTO> aplicativos
);