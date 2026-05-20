using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class AplicativoUseCases(Context context)
{
    

    public Result<Aplicativo> criar(AplicativoDTO aplicativoDto)
    {   
        var aplicativo = new Aplicativo(aplicativoDto.nome,
            aplicativoDto.versao,
            aplicativoDto.descricao);
        context.Aplicativos.Add(aplicativo);
        context.SaveChanges();
        
        return Result<Aplicativo>.Success(aplicativo,200);
    }
    
    public Result<Aplicativo> procurarUm(int id)
    {
        var aplicativo = context.Aplicativos.Find(id);
        if (aplicativo == null)
        {
            return Result<Aplicativo>.Failure("Aplicativo não encontrado",404);}
        return Result<Aplicativo>.Success(aplicativo,200);
    }
    public Result<ListaDeAplicativosDTO> listar(int pagina, int quantidade)
    {
        var aplicativos = context.Aplicativos.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();
        
        return Result<ListaDeAplicativosDTO>.Success(new ListaDeAplicativosDTO(pagina, quantidade, aplicativos),200);
    }
    public Result<Aplicativo> remover(int id)
    {
        context.Aplicativos.Where(a => a.id == id).ExecuteDelete();
        return Result<Aplicativo>.NoContent(204);
    }
    
    public Result<Aplicativo> atualizar(int id, AplicativoDTO aplicativoAtualizadoDto)
    {
        var aplicativoAtualizado = new Aplicativo(id, aplicativoAtualizadoDto.nome, aplicativoAtualizadoDto.versao,
            aplicativoAtualizadoDto.descricao);
        context.Aplicativos.Update(aplicativoAtualizado);
        context.SaveChanges();
        
        return Result<Aplicativo>.Success(aplicativoAtualizado,200);
    }

}