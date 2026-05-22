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
        return Result<Laboratorio>.Success(laboratorio,201);
    }
    public Result<Laboratorio> procurarUm(int id)
    {
        var laboratorio = context.Laboratorios.Find(id);
        if (laboratorio == null)
        {
            return Result<Laboratorio>.Failure("Alocação não encontrada",404);
        }
        return Result<Laboratorio>.Success(laboratorio,200);
    }
    public Result<ListaDeLaboratorioDTO> listar(int pagina, int quantidade)
    {
        var laboratorios = context.Laboratorios.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();
        
        return Result<ListaDeLaboratorioDTO>.Success(new ListaDeLaboratorioDTO(pagina, quantidade, laboratorios),200);
    }
    public Result<Laboratorio> remover(int id)
    {
        context.Laboratorios.Where(l => l.id == id).ExecuteDelete();
        return Result<Laboratorio>.NoContent(204);
    }
    
    public Result<Laboratorio> atualizar(int id,LaboratorioDTO laboratorioDto)
    {
        var laboratorio = new Laboratorio(id, laboratorioDto.nome, laboratorioDto.qnt_computadores);
        context.Laboratorios.Update(laboratorio);
        context.SaveChanges();
        return Result<Laboratorio>.Success(laboratorio,200);
    }

    public Result<string> vincular(VincularAppsNoLaboratorioDTO laboratorioDto)
    {
        if (!context.Laboratorios.Any(l => l.id == laboratorioDto.laboratorioId))
        {
            return Result<string>.Failure("Laboratorio não encontrado", 404);
        }

        var vinculacoes =
            laboratorioDto.idsDeAplicativos.Select(appId =>
                new Laboratorio_Aplicativo(laboratorioDto.laboratorioId, appId)).ToList();
        context.LaboratorioAplicativos.AddRange(vinculacoes);
        context.SaveChanges();
        
        return Result<string>.Success("Aplicativos vinculados ao laboratorio",200);

    }
 
}