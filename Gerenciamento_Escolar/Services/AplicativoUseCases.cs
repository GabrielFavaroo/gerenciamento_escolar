using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;

namespace Gerenciamento_Escolar.Services;

public class AplicativoUseCases(Context context)
{
    

    public Aplicativo criar(AplicativoDTO aplicativoDto)
    {   
        var aplicativo = new Aplicativo(aplicativoDto.nome,
            aplicativoDto.versao,
            aplicativoDto.descricao);
        context.Aplicativos.Add(aplicativo);
        aplicativo = context.Aplicativos.Find(aplicativo);
        return aplicativo;
    }
    public Aplicativo procurarUm(int id)
    {
        var aplicativo = context.Aplicativos.Find(id);
        if (aplicativo == null)
        {
            throw new Exception("Aplicativo não encontrado");}
        return aplicativo;
    }
    public ListaDeAplicativosDTO listar(int pagina, int quantidade)
    {
        var aplicativos = context.Aplicativos.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();
        return new ListaDeAplicativosDTO(pagina,quantidade,aplicativos);
    }
    public void remover(int id)
    {
        
        context.Aplicativos.Remove(this.procurarUm(id));
    }
    
    public Aplicativo atualizar(int id, AplicativoDTO aplicativoAtualizadoDto)
    {
        var aplicativoAtualizado = new Aplicativo(id, aplicativoAtualizadoDto.nome, aplicativoAtualizadoDto.versao,
            aplicativoAtualizadoDto.descricao);
        context.Aplicativos.Update(aplicativoAtualizado);
        var aplicativo = context.Aplicativos.Find(id);
        return aplicativo;
    }

}