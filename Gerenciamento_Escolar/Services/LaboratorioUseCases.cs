using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class LaboratorioUseCases(Context context)
{
    

    public Laboratorio criar(LaboratorioDTO laboratorioDto)
    {
        var laboratorio = new Laboratorio(laboratorioDto.nome, laboratorioDto.qnt_computadores);
        context.Laboratorios.Add(laboratorio);
        context.SaveChanges();
        return laboratorio;
    }
    public Laboratorio procurarUm(int id)
    {
        var laboratorio = context.Laboratorios.Find(id);
        if (laboratorio == null)
        {
            throw new ("Alocação não encontrada");
        }
        return laboratorio;
    }
    public ListaDeLaboratorioDTO listar(int pagina, int quantidade)
    {
        var laboratorios = context.Laboratorios.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();
        
        return new ListaDeLaboratorioDTO(pagina,quantidade,laboratorios);
    }
    public void remover(int id)
    {
        context.Laboratorios.Where(l => l.id == id).ExecuteDelete();
    }
    
    public Laboratorio atualizar(int id,LaboratorioDTO laboratorioDto)
    {
        var laboratorio = new Laboratorio(id, laboratorioDto.nome, laboratorioDto.qnt_computadores);
        context.Laboratorios.Update(laboratorio);
        context.SaveChanges();
        return laboratorio;
    }

}