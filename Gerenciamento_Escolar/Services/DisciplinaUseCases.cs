using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;

namespace Gerenciamento_Escolar.Services;

public class DisciplinaUseCases(Context context)
{
    

    public Disciplina criar(DisciplinaDTO disciplinaDto)
    {
        var disciplina = new Disciplina(disciplinaDto.nome,
            disciplinaDto.alunos_matriculados,
            disciplinaDto.descricao,
            disciplinaDto.coordenador_id);
        context.Disciplinas.Add(disciplina);
        disciplina = context.Disciplinas.Find(disciplina);
        return disciplina;
    }
    public Disciplina procurarUm(int id)
    {
        var disciplina = context.Disciplinas.Find(id);
        if (disciplina == null)
        {
            throw new Exception("Disciplina não encontrada");
        }
        return disciplina;
    }
    public ListaDeDisciplinaDTO listar(int pagina, int quantidade)
    {
        
          var disciplinas =  context.Disciplinas.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();

          return new ListaDeDisciplinaDTO(pagina, quantidade, disciplinas);
    }
    public void remover(int id)
    {
        
        context.Disciplinas.Remove(procurarUm(id));
        
    }
    
    public Disciplina atualizar(int id, DisciplinaDTO disciplinaAtualizadaDto)
    {
        var disciplinaAtualizada = new Disciplina(id, disciplinaAtualizadaDto.nome,
            disciplinaAtualizadaDto.alunos_matriculados, disciplinaAtualizadaDto.descricao,
            disciplinaAtualizadaDto.coordenador_id);
        context.Disciplinas.Update(disciplinaAtualizada);
        var disciplina = context.Disciplinas.Find(id);
        return disciplina;
    }

}