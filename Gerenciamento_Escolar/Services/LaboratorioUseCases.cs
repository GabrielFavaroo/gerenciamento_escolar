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
        var laboratorio = new Laboratorio(id, laboratorioDto.nome, laboratorioDto.qnt_computadores);
        context.Laboratorios.Update(laboratorio);
        context.SaveChanges();
        return Result<Laboratorio>.Success(laboratorio, 200);
    }

    public Result<string> vincular(VincularAppsNoLaboratorioDTO laboratorioDto)
    {

            if (!context.Laboratorios.Any(la => la.id == laboratorioDto.laboratorioId))
            {
                return Result<string>.Failure("Laboratorio não encontrado", 404);
            }

            var appsLimpos = laboratorioDto.idsDeAplicativos.Distinct().ToList();

            if (appsLimpos.Any())
            {
                var appsEncontrados = context.Aplicativos.Count(a => appsLimpos.Contains(a.id));
                if (appsEncontrados != appsLimpos.Count)
                {
                    return Result<string>.Failure("Um ou mais aplicativos não foram encontrados", 404);
                }

            }

            var appsExistentes = context.LaboratorioAplicativos
                .Where(da => da.laboratorio_id == laboratorioDto.laboratorioId).ToList();

            var idsAtuais = appsExistentes.Select(e => e.aplicativo_id).ToList();

            var appsRemover = appsExistentes.Where(atual => !appsLimpos.Contains(atual.aplicativo_id)).ToList();


            var add = appsLimpos
                .Where(id => !idsAtuais.Contains(id))
                .Select(id =>
                    new Laboratorio_Aplicativo(laboratorio_id: laboratorioDto.laboratorioId, id)).ToList();

            if (appsRemover.Any()) context.LaboratorioAplicativos.RemoveRange(appsRemover);

            if (add.Any()) context.LaboratorioAplicativos.AddRange(add);

            context.SaveChanges();

            return Result<string>.Success("Aplicativos vinculados no laboratorio", 200);
        
    }
    
    
    public Result<ListaDeAplicativosVinculadosDTO> consultarApps(int id, int pagina, int quantidade)
    {
        var laboratorio = context.Laboratorios.Find(id);
        if (laboratorio == null)
        {
            return Result<ListaDeAplicativosVinculadosDTO>.Failure("Laboratorio não encontrado", 404);
        }
        
        var apps = context.LaboratorioAplicativos
            .Where(l => l.laboratorio_id == id)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .Select(da => new AplicativoVinculadoDTO(
                da.aplicativo_id,
                da.aplicativo.nome)).ToList();
            
            
        
        return Result<ListaDeAplicativosVinculadosDTO>.Success(new ListaDeAplicativosVinculadosDTO(pagina,quantidade, apps),200);
    }
}
    

