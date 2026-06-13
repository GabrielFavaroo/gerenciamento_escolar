namespace Gerenciamento_Escolar.Dtos;

public record ListaGeralLaboratoriosComAppsDTO(
    int pagina,
    int quantidade,
    List<LaboratorioItensComAppsDTO> laboratorios);