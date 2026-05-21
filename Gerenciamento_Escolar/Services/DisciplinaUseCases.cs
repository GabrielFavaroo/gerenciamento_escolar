using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class DisciplinaUseCases(Context context)
{
    

    public Result<Disciplina> criar(DisciplinaDTO disciplinaDto)
    {
        var disciplina = new Disciplina(disciplinaDto.nome,
            disciplinaDto.alunos_matriculados,
            disciplinaDto.descricao,
            disciplinaDto.coordenador_id);
        context.Disciplinas.Add(disciplina);
        context.SaveChanges();
        
        return Result<Disciplina>.Success(disciplina,201);
    }
    public Result<Disciplina> procurarUm(int id)
    {
        var disciplina = context.Disciplinas.Find(id);
        if (disciplina == null)
        {
            Result<Disciplina>.Failure("Disciplina não encontrada",404);
        }
        return Result<Disciplina>.Success(disciplina,200);
    }
    public Result<ListaDeDisciplinaDTO> listar(int pagina, int quantidade)
    {
        
          var disciplinas =  context.Disciplinas.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();

          return Result<ListaDeDisciplinaDTO>.Success(new ListaDeDisciplinaDTO(pagina, quantidade, disciplinas),200);
    }
    public Result<Disciplina> remover(int id)
    {
        
        context.Disciplinas.Where(d => d.id == id).ExecuteDelete();
        return Result<Disciplina>.NoContent(204);
    }
    
    public Result<Disciplina> atualizar(int id, DisciplinaDTO disciplinaAtualizadaDto)
    {
        var disciplinaAtualizada = new Disciplina(id, disciplinaAtualizadaDto.nome,
            disciplinaAtualizadaDto.alunos_matriculados, disciplinaAtualizadaDto.descricao,
            disciplinaAtualizadaDto.coordenador_id);
        context.Disciplinas.Update(disciplinaAtualizada);
        context.SaveChanges();
        return Result<Disciplina>.Success(disciplinaAtualizada,200);
        
    }

}