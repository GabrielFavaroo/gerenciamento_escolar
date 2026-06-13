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

        var coordenador =
            context.Usuarios.FirstOrDefault(u => u.nome == disciplinaDto.nome && u.tipoUsuario == "Coordenador");
        
        if(coordenador == null)
        {
            return Result<Disciplina>.Failure("Usuario invalido", 404);
        }
        
        var disciplina = new Disciplina(disciplinaDto.nome,
            disciplinaDto.alunos_matriculados,
            disciplinaDto.descricao,disciplinaDto.duracao_meses,disciplinaDto.horario_inicio_aula,disciplinaDto.horario_fim_aula,
            coordenador.id);
        context.Disciplinas.Add(disciplina);
        
        
        context.SaveChanges();
        
        return Result<Disciplina>.Success(disciplina,201);
    }
    public Result<Disciplina> procurarUm(int id)
    {
        var disciplina = context.Disciplinas.Find(id);
        if (disciplina == null)
        {
            return Result<Disciplina>.Failure("Disciplina não encontrada",404);
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

        var disciplina = context.Disciplinas.Find(id);
        if (disciplina == null)
        {
            return Result<Disciplina>.Failure("Disciplina não encontrada para atualização", 404);
        }

        var coordenador = context.Usuarios.FirstOrDefault(u => u.nome == disciplinaAtualizadaDto.nomeCoordenador && u.tipoUsuario == "Coordenador");
        
        if (coordenador == null)
        {
            return Result<Disciplina>.Failure("Usuário coordenador inválido ou não encontrado", 404);
        }
        
        disciplina.nome = disciplinaAtualizadaDto.nome;
        disciplina.alunos_matriculados = disciplinaAtualizadaDto.alunos_matriculados;
        disciplina.descricao = disciplinaAtualizadaDto.descricao;
        disciplina.duracao_meses = disciplinaAtualizadaDto.duracao_meses;
        disciplina.horario_inicio_aula = disciplinaAtualizadaDto.horario_inicio_aula;
        disciplina.horario_fim_aula = disciplinaAtualizadaDto.horario_fim_aula;
        disciplina.coordenador_id = coordenador.id; 
        context.Disciplinas.Update(disciplina);
        context.SaveChanges();
        return Result<Disciplina>.Success(disciplina,200);
        
    }
    
    

    public Result<string> vincular(VincularAppsNaDisciplinaDTO disciplinaDto)
    {
        var disciplina = context.Disciplinas.FirstOrDefault(d => d.nome == disciplinaDto.disciplina);
        
        if (disciplina == null)
        {
            return Result<string>.Failure("Disciplina não encontrada", 404);
        }

        var appsLimpos = disciplinaDto.aplicativos.Distinct().ToList();

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

            var idsAppsJaVinculados = context.DisciplinaAplicativos.Where(da => da.disciplina_id == disciplina.id)
                .Select(da => da.aplicativo_id).ToList();
            
            var idsDosApps = appsNoBanco.Select(a => a.id).ToList();

            var appsRestantes = idsDosApps.Except(idsAppsJaVinculados);
            
            foreach (var idApp in appsRestantes)
            {
                var vinculo = new Disciplina_Aplicativo(disciplina.id, idApp);
                
                context.DisciplinaAplicativos.Add(vinculo);
            }
        }
        
        
        context.SaveChanges();
        
        return Result<string>.Success("Aplicativos vinculados na disciplina", 200);
    }


    public Result<ListaGeralDisciplinasComAppsDTO> consultarApps(int pagina, int quantidade)
    {
        var disciplinasComApps = context.Disciplinas
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .Select(d => new DisciplinaItemComAppsDTO(
                d.id,
                d.nome,
                d.descricao,
                context.DisciplinaAplicativos
                    .Where(da => da.disciplina_id == d.id)
                    .Select(da => new AplicativoVinculadoDTO(
                        da.aplicativo_id,
                        da.aplicativo.nome
                    ))
                    .ToList()
            ))
            .ToList();

        var resultado = new ListaGeralDisciplinasComAppsDTO(pagina, quantidade, disciplinasComApps);
    
        return Result<ListaGeralDisciplinasComAppsDTO>.Success(resultado, 200);
    }

}