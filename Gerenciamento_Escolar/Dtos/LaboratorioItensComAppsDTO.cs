namespace Gerenciamento_Escolar.Dtos;

public record LaboratorioItensComAppsDTO(int id,
    string nome,
    int quantidade_computadores,
    List<AplicativoVinculadoDTO> aplicativos);