using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class LaboratorioUseCases(Context context)
{


    public Result<Laboratorio> criar(LaboratorioDTO laboratorioDto)
    {
        var laboratorio = new Laboratorio(laboratorioDto.nome, laboratorioDto.qnt_computadores);
        context.Laboratorios.Add(laboratorio);
        context.SaveChanges();
        return Result<Laboratorio>.Success(laboratorio, 201);
    }

    public Result<Laboratorio> procurarUm(int id)
    {
        var laboratorio = context.Laboratorios.Find(id);
        if (laboratorio == null)
        {
            return Result<Laboratorio>.Failure("Laboratorio não encontrado", 404);
        }

        return Result<Laboratorio>.Success(laboratorio, 200);
    }

    public Result<ListaDeLaboratorioDTO> listar(int pagina, int quantidade)
    {
        var laboratorios = context.Laboratorios.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();

        return Result<ListaDeLaboratorioDTO>.Success(new ListaDeLaboratorioDTO(pagina, quantidade, laboratorios), 200);
    }

    public Result<Laboratorio> remover(int id)
    {
        context.Laboratorios.Where(l => l.id == id).ExecuteDelete();
        return Result<Laboratorio>.NoContent(204);
    }

    public Result<Laboratorio> atualizar(int id, LaboratorioDTO laboratorioDto)
    {
        var laboratorio = context.Laboratorios.FirstOrDefault(l => l.id == id);
        laboratorio.qnt_computadores = laboratorioDto.qnt_computadores;
        laboratorio.nome = laboratorioDto.nome;
            context.Laboratorios.Update(laboratorio);
        context.SaveChanges();
        return Result<Laboratorio>.Success(laboratorio, 200);
    }

    public Result<string> vincular(VincularAppsNoLaboratorioDTO laboratorioDto)
    {
        var laboratorio = context.Laboratorios.FirstOrDefault(d => d.nome == laboratorioDto.laboratorio);
        
        if (laboratorio == null)
        {
            return Result<string>.Failure("Laboratorio não encontrado", 404);
        }

        var appsLimpos = laboratorioDto.aplicativos.Distinct().ToList();

        if (appsLimpos.Any())
        {
            var appsNoBanco = context.Aplicativos
                .Where(app => appsLimpos.Contains(app.nome))
                .Select(app => new { app.id, app.nome })
                .ToList();

            if (appsNoBanco.Count != appsLimpos.Count)
            {
                var nomesEncontrados = appsNoBanco.Select(a => a.nome).ToList();
                var appFaltante = appsLimpos.FirstOrDefault(nome => !nomesEncontrados.Contains(nome));

                return Result<string>.Failure($"O aplicativo '{appFaltante}' não foi encontrado no sistema", 404);
            }

            var idsAppsJaVinculados = context.LaboratorioAplicativos.Where(da => da.laboratorio_id == laboratorio.id)
                .Select(da => da.aplicativo_id).ToList();
            
            var idsDosApps = appsNoBanco.Select(a => a.id).ToList();

            var appsRestantes = idsDosApps.Except(idsAppsJaVinculados);
            
            foreach (var idApp in appsRestantes)
            {
                var vinculo = new Laboratorio_Aplicativo(laboratorio.id, idApp);
                
                context.LaboratorioAplicativos.Add(vinculo);
            }
        }
        
        
        context.SaveChanges();

            return Result<string>.Success("Aplicativos vinculados no laboratorio", 200);
        
    }
    
    
    public Result<ListaGeralLaboratoriosComAppsDTO> consultarApps(int pagina, int quantidade)
    {
        var labsComApps = context.Laboratorios
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .Select(d => new LaboratorioItensComAppsDTO(
                d.id,
                d.nome,
                d.qnt_computadores,
                context.LaboratorioAplicativos
                    .Where(la => la.laboratorio_id == d.id)
                    .Select(da => new AplicativoVinculadoDTO(
                        da.aplicativo_id,
                        da.aplicativo.nome
                    ))
                    .ToList()
            ))
            .ToList();

        var resultado = new ListaGeralLaboratoriosComAppsDTO(pagina, quantidade, labsComApps);
    
        return Result<ListaGeralLaboratoriosComAppsDTO>.Success(resultado, 200);}
}
    

