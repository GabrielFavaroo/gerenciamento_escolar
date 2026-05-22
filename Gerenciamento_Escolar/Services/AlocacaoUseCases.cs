using System.Collections;
using System.Text.RegularExpressions;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class AlocacaoUseCases(Context context)
{
    private static HashSet<String> nomesDosDias = new HashSet<string>()
    {
        "segunda","terca","quarta","quinta","sexta"
    };

    private static string pattern = @"[ -_][Ff]eira|\s|\d|ç|Ç";
    

    public Result<Alocacao> Criar(AlocacaoDTO alocacaoDto)
    {
        if (!context.Laboratorios.Any(l => l.id == alocacaoDto.laboratorio_id) ||
            !context.Disciplinas.Any(d => d.id == alocacaoDto.disciplina_id))
        {
            return Result<Alocacao>.Failure("Laboratorio ou disciplina informados não foram encontrados",404);
        }
        

        if(!DateIsFree(alocacaoDto.dataAgendamento,alocacaoDto.horario_inicio,alocacaoDto.horario_fim))
            return Result<Alocacao>.Failure("O horario da alocação já esta reservado",409);


        var diaFormatado = formatDateName(alocacaoDto.dia_da_semana);


        if (!IsTheDayValid(diaFormatado))
        {
            return Result<Alocacao>.Failure("Dia da semana inválido",400);
        };
        
        if (!context.Usuarios.Any(u => u.id == alocacaoDto.coordenadorId))
        {
            return Result<Alocacao>.Failure("Usuario não encontrado", 404);
        }
        if (context.Usuarios.Any(u => u.id == alocacaoDto.coordenadorId && u.tipoUsuario != "Coordenador"))
        {
            return Result<Alocacao>.Failure("O usuario informado não é um coordenador", 409);
        }


        var alocacao = new Alocacao(alocacaoDto.disciplina_id,
            alocacaoDto.laboratorio_id,
            diaFormatado,
            alocacaoDto.dataAgendamento,
            alocacaoDto.horario_inicio,
            alocacaoDto.horario_fim,
            nameof(StatusAlocacao.Pendente),
            null,
            null,
            alocacaoDto.coordenadorId
            );
        
        context.Alocacoes.Add(alocacao);
        context.SaveChanges();
        return Result<Alocacao>.Success(alocacao,201);
    }

    public Result<Alocacao> ProcurarUm(int id)
    {
        
        var alocacao = context.Alocacoes.Find(id);
        if (alocacao == null)
        {
            return Result<Alocacao>.Failure("Alocação não encontrada",404);
        }
        return Result<Alocacao>.Success(alocacao,200);
    }
    
    public Result<ListaDeAlocacoesDTO> Listar(int pagina, int quantidade)
    {
        var alocacoes = context.Alocacoes.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();
        
        
        return Result<ListaDeAlocacoesDTO>.Success(new ListaDeAlocacoesDTO(pagina, quantidade, alocacoes),200);
    }
    public Result<Alocacao> remover(int id)
    {

        context.Alocacoes.Where(a => a.id == id).ExecuteDelete();
        return Result<Alocacao>.NoContent(204);
    }
    
    public Result<Alocacao> Atualizar(int id,AlocacaoAtualizadaDTO alocacaoAtualizadaDto)
    {   
        
        if (!context.Laboratorios.Any(l => l.id == alocacaoAtualizadaDto.laboratorio_id) ||
              !context.Disciplinas.Any(d => d.id == alocacaoAtualizadaDto.disciplina_id))
        {
            return Result<Alocacao>.Failure("Laboratorio ou disciplina informados não foram encontrados",404);
        }
        
        
        if(!DateIsFree(alocacaoAtualizadaDto.dataAgendamento,alocacaoAtualizadaDto.horario_inicio,alocacaoAtualizadaDto.horario_fim))
            return Result<Alocacao>.Failure("O horario da alocação já estava reservado",409);

        
        var diaFormatado = Regex.Replace(input: alocacaoAtualizadaDto.dia_da_semana,pattern, replacement: "").ToLower();


        if (!IsTheDayValid(diaFormatado))
        {
            return Result<Alocacao>.Failure("Dia da semana inválido",400);
        };
        
        if (!context.Usuarios.Any(u => u.id == alocacaoAtualizadaDto.coordenadorId))
        {
            return Result<Alocacao>.Failure("Usuario não encontrado", 404);
        }
        if (context.Usuarios.Any(u => u.id == alocacaoAtualizadaDto.coordenadorId && u.tipoUsuario != "Coordenador"))
        {
            return Result<Alocacao>.Failure("O usuario informado não é um coordenador", 409);
        }
        
        var alocacaoAtualizada = new Alocacao(id, alocacaoAtualizadaDto.disciplina_id,
            alocacaoAtualizadaDto.laboratorio_id,
            diaFormatado,alocacaoAtualizadaDto.dataAgendamento,
            alocacaoAtualizadaDto.horario_inicio, alocacaoAtualizadaDto.horario_fim,
            alocacaoAtualizadaDto.status,
            alocacaoAtualizadaDto.aprovadoPorId,
            alocacaoAtualizadaDto.dataAprovacao, alocacaoAtualizadaDto.coordenadorId);

        if (!Enum.TryParse<StatusAlocacao>(alocacaoAtualizadaDto.status, true, out StatusAlocacao resultado))
        {
            alocacaoAtualizada.status = nameof(StatusAlocacao.Pendente);
        }
        
        
        if (!labComportsStudents(alocacaoAtualizadaDto) || !labHasTheRequiredApps(alocacaoAtualizadaDto))
        {
            alocacaoAtualizada.status = nameof(StatusAlocacao.Negado);
        }
        else
        {
            alocacaoAtualizada.status = nameof(StatusAlocacao.Aprovado);
        }

        if (alocacaoAtualizada.status == nameof(StatusAlocacao.Aprovado))
        {
            if (!context.Usuarios.Any(u => u.id == alocacaoAtualizada.aprovado_por_id))
            {
                return Result<Alocacao>.Failure("Usuario não encontrado", 404);
            }
            if (context.Usuarios.Any(u => u.id == alocacaoAtualizada.aprovado_por_id && u.tipoUsuario != "Diretor"))
            {
                return Result<Alocacao>.Failure("O usuario informado não é um diretor", 409);
            }
            
            alocacaoAtualizada.data_aprovacao = DateTime.Now;
        }
        else
        {
            alocacaoAtualizada.aprovado_por_id = null;
            alocacaoAtualizada.data_aprovacao = null;
        }
        
        context.Alocacoes.Update(alocacaoAtualizada);
        context.SaveChanges();
        
        
        return Result<Alocacao>.Success(alocacaoAtualizada,200);
    }
    
    

    private bool IsTheDayValid(string diaFormatado)
    {
        return nomesDosDias.Contains(diaFormatado) ? true : false;
        
        
    }

    private bool DateIsFree(DateTime date, TimeSpan startTime,TimeSpan endTime)
    {
        if (context.Alocacoes.Any(al =>
                al.data_agendamento == date
                && startTime > al.horario_fim || al.horario_inicio > endTime)
           )
        {
            return false;
        }

        return true;
    }

    private string formatDateName(string dateName)
    {
        return Regex.Replace(dateName,pattern, replacement: "").ToLower();
    }

    private bool labHasTheRequiredApps(AlocacaoAtualizadaDTO alocacaoAtualizadaDto)
    {
        var appsrequeridos = context.DisciplinaAplicativos
            .Where(da => da.disciplina_id == alocacaoAtualizadaDto.disciplina_id)
            .Select(da => da.aplicativo_id).ToList();
        
        if (!appsrequeridos.Any())
        {
            return true;
        }

        var appsnolab = context.LaboratorioAplicativos
            .Where(la => la.laboratorio_id == alocacaoAtualizadaDto.laboratorio_id)
            .Select(la => la.aplicativo_id).ToList();

        
        return appsrequeridos.All(requeridos => appsnolab.Contains(requeridos));
        
    }

    private bool labComportsStudents(AlocacaoAtualizadaDTO alocacaoAtualizadaDto)
    {
        var pcs = context.Laboratorios
            .Where(l => l.id == alocacaoAtualizadaDto.laboratorio_id)
            .Select(l => l.qnt_computadores).FirstOrDefault();

        var alunos = context.Disciplinas
            .Where(d => d.id == alocacaoAtualizadaDto.disciplina_id)
            .Select(d => d.alunos_matriculados).FirstOrDefault();

        if (pcs == 0)
        {
            return false;}
        var maxStudentsPerPc = 2.0;
        
        return !(((double)alunos / pcs) > maxStudentsPerPc);


    }
}