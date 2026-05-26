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
        if (!TimeService.IsTimePeriodCorrect(disciplinaDto.horario_inicio_aula, disciplinaDto.horario_fim_aula))
        {
            return Result<Disciplina>.Failure("É impossivel criar uma disciplina com horario de inicio da aula maior ou igual ao horario de fim", 422);
        }
        
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
            disciplinaDto.descricao,disciplinaDto.duracao_meses,disciplinaDto.horario_inicio_aula,disciplinaDto.horario_fim_aula,
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
        if (!TimeService.IsTimePeriodCorrect(disciplinaAtualizadaDto.horario_inicio_aula, disciplinaAtualizadaDto.horario_fim_aula))
        {
            return Result<Disciplina>.Failure("É impossivel criar uma disciplina com horario de inicio da aula maior ou igual ao horario de fim", 422);
        }
        
        if (!context.Usuarios.Any(u => u.id == disciplinaAtualizadaDto.coordenador_id))
        {
            return Result<Disciplina>.Failure("Usuario não encontrado", 404);
        }
        if (context.Usuarios.Any(u => u.id == disciplinaAtualizadaDto.coordenador_id && u.tipoUsuario != "Coordenador"))
        {
            return Result<Disciplina>.Failure("O usuario informado não é um coordenador", 409);
        }
        
        
        var disciplinaAtualizada = new Disciplina(id, disciplinaAtualizadaDto.nome,
            disciplinaAtualizadaDto.alunos_matriculados, disciplinaAtualizadaDto.descricao,disciplinaAtualizadaDto.duracao_meses,disciplinaAtualizadaDto.horario_inicio_aula,disciplinaAtualizadaDto.horario_fim_aula,
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

        var appsLimpos = disciplinaDto.idsDeAplicativos.Distinct().ToList();

        if (appsLimpos.Any())
        {
            var appsEncontrados = context.Aplicativos.Count(a => appsLimpos.Contains(a.id));
            if (appsEncontrados != appsLimpos.Count)
            {
                return Result<string>.Failure("Um ou mais aplicativos não foram encontrados", 404);
            }

        }
        
        var appsExistentes = context.DisciplinaAplicativos
            .Where(da => da.disciplina_id == disciplinaDto.disciplinaId).ToList();

        var idsAtuais = appsExistentes.Select(e => e.aplicativo_id).ToList();

        var appsRemover = appsExistentes.
            Where(atual => !appsLimpos.Contains(atual.aplicativo_id)).ToList();

        
        var add = appsLimpos
            .Where(id => !idsAtuais.Contains(id))
            .Select(id => 
                new Disciplina_Aplicativo(disciplina_id: disciplinaDto.disciplinaId, id)).ToList();
        
        if(appsRemover.Any()) context.DisciplinaAplicativos.RemoveRange(appsRemover);
        
        if(add.Any()) context.DisciplinaAplicativos.AddRange(add);
        
        context.SaveChanges();
        
        return Result<string>.Success("Aplicativos vinculados na disciplina", 200);
    }


    public Result<ListaDeAplicativosVinculadosDTO> consultarApps(int id, int pagina, int quantidade)
    {
        
        var disciplina = context.Disciplinas.Find(id);
        if (disciplina == null)
        {
            return Result<ListaDeAplicativosVinculadosDTO>.Failure("Disciplina não encontrada", 404);
        }
        
        var apps = context.DisciplinaAplicativos
            .Where(d => d.disciplina_id == id)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .Select(da => new AplicativoVinculadoDTO(
                da.aplicativo_id,
                da.aplicativo.nome)).ToList();
            
            
        
        return Result<ListaDeAplicativosVinculadosDTO>.Success(new ListaDeAplicativosVinculadosDTO(pagina,quantidade, apps),200);
    }

}