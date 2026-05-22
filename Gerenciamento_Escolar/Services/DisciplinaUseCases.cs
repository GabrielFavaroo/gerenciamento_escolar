using System.Net;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class DisciplinaUseCases(Context context)
{
    

    public Result<Disciplina> criar(DisciplinaDTO disciplinaDto)
    {
        if (!context.Usuarios.Any(u => u.id == disciplinaDto.coordenador_id))
        {
            return Result<Disciplina>.Failure("Usuario não encontrado", 404);
        }
        if (context.Usuarios.Any(u => u.id == disciplinaDto.coordenador_id && u.tipoUsuario != "Coordenador"))
        {
            return Result<Disciplina>.Failure("O usuario informado não é um coordenador", 409);
        }
        
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
        if (!context.Usuarios.Any(u => u.id == disciplinaAtualizadaDto.coordenador_id))
        {
            return Result<Disciplina>.Failure("Usuario não encontrado", 404);
        }
        if (context.Usuarios.Any(u => u.id == disciplinaAtualizadaDto.coordenador_id && u.tipoUsuario != "Coordenador"))
        {
            return Result<Disciplina>.Failure("O usuario informado não é um coordenador", 409);
        }
        
        
        var disciplinaAtualizada = new Disciplina(id, disciplinaAtualizadaDto.nome,
            disciplinaAtualizadaDto.alunos_matriculados, disciplinaAtualizadaDto.descricao,
            disciplinaAtualizadaDto.coordenador_id);
        context.Disciplinas.Update(disciplinaAtualizada);
        context.SaveChanges();
        return Result<Disciplina>.Success(disciplinaAtualizada,200);
        
    }
    
    

    public Result<string> vincular(VincularAppsNaDisciplinaDTO disciplinaDto)
    {
        if (!context.Disciplinas.Any(d => d.id == disciplinaDto.disciplinaId))
        {
            return Result<string>.Failure("Disciplina não encontrada", 404);
        }

        var vinculacoes =
            disciplinaDto.idsDeAplicativos.Select(appId => new Disciplina_Aplicativo(disciplinaDto.disciplinaId, appId)).ToList();

        context.DisciplinaAplicativos.AddRange(vinculacoes);
        context.SaveChanges();
        
        return Result<string>.Success("Aplicativos vinculados na disciplina", 200);
    }

}