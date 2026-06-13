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
        var disciplina = context.Disciplinas.FirstOrDefault(d => d.nome == alocacaoDto.disciplina_nome);
        
        if (disciplina == null)
        {
            return Result<Alocacao>.Failure("A disciplina informada não foi encontrada",404);
        }
        
        var laboratorio = context.Laboratorios.FirstOrDefault(l => l.nome == alocacaoDto.laboratorio_nome);

        if (laboratorio == null)
        {
            return Result<Alocacao>.Failure("O laboratorio informado não foi encontrado",404);
        }
        
        var coordenador = context.Usuarios.FirstOrDefault(u => u.nome == alocacaoDto.coordenador_nome && u.tipoUsuario == "Coordenador");

        if (coordenador == null)
        {
            return Result<Alocacao>.Failure("Usuario invalido", 409);
        }

        if (!TimeService.IsTimePeriodCorrect(alocacaoDto.horario_inicio, alocacaoDto.horario_fim))
        {
            return Result<Alocacao>.Failure(
                "É impossivel criar uma alocação com o horario de inicio maior ou igual ao de fim",422);
        }


        if (!DateIsFree(alocacaoDto.dataAgendamento, alocacaoDto.horario_inicio, alocacaoDto.horario_fim,laboratorio.id,null))
        {
            return Result<Alocacao>.Failure("O horario da alocação já esta reservado", 409);
        }


        var diaFormatado = formatDateName(alocacaoDto.dia_da_semana);


        if (!IsTheDayValid(diaFormatado))
        {
            return Result<Alocacao>.Failure("Dia da semana inválido",400);
        };
        
        


        var alocacao = new Alocacao(alocacaoDto.titulo,disciplina.id,
            laboratorio.id,
            diaFormatado,
            alocacaoDto.dataAgendamento,
            alocacaoDto.horario_inicio,
            alocacaoDto.horario_fim,
            nameof(StatusAlocacao.Pendente),
            null,
            null,
            coordenador.id
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
        var laboratorio = context.Laboratorios.FirstOrDefault(l => l.nome == alocacaoAtualizadaDto.laboratorio_nome);

        if (laboratorio == null)
        {
            return Result<Alocacao>.Failure("O laboratorio informado não foi encontrado",404);
        }

        var disciplina = context.Disciplinas.FirstOrDefault(d => d.nome == alocacaoAtualizadaDto.disciplina_nome);    

        if (disciplina == null)
        {
            return Result<Alocacao>.Failure("A disciplina informada não foi encontrada",404);
        }
        
        if (!TimeService.IsTimePeriodCorrect(alocacaoAtualizadaDto.horario_inicio, alocacaoAtualizadaDto.horario_fim))
        {
            return Result<Alocacao>.Failure(
                "É impossivel criar uma alocação com o horario de inicio maior ou igual ao de fim",422);
        }

        if (!DateIsFree(alocacaoAtualizadaDto.dataAgendamento, alocacaoAtualizadaDto.horario_inicio,
                alocacaoAtualizadaDto.horario_fim,laboratorio.id,id))
        {
            return Result<Alocacao>.Failure("O horario da alocação já estava reservado", 409);
        }

        var diaFormatado = Regex.Replace(input: alocacaoAtualizadaDto.dia_da_semana,pattern, replacement: "").ToLower();


        if (!IsTheDayValid(diaFormatado))
        {
            return Result<Alocacao>.Failure("Dia da semana inválido",400);
        };



        var alocacaoAtualizada = context.Alocacoes.Find(id);

        alocacaoAtualizada.titulo = alocacaoAtualizadaDto.titulo;
        alocacaoAtualizada.disciplina_id = disciplina.id;
        alocacaoAtualizada.laboratorio_id = laboratorio.id;
        alocacaoAtualizada.dia_da_semana = diaFormatado;
        alocacaoAtualizada.data_agendamento = alocacaoAtualizadaDto.dataAgendamento;
        alocacaoAtualizada.horario_inicio = alocacaoAtualizadaDto.horario_inicio;
        alocacaoAtualizada.horario_fim = alocacaoAtualizadaDto.horario_fim;
        
        
        
        context.Alocacoes.Update(alocacaoAtualizada);
        context.SaveChanges();
        
        
        return Result<Alocacao>.Success(alocacaoAtualizada,200);
    }
    
    public Result<Alocacao> AtualizarStatus(int id, string status,string nome)
    {
        var alocacao = context.Alocacoes.FirstOrDefault(a => a.id == id);
        
        if (Enum.TryParse<StatusAlocacao>(status, true, out StatusAlocacao resultado))
        {
            status = resultado.ToString();
        }
        else
        {
            status = nameof(StatusAlocacao.Pendente);
        }

        alocacao.status = status;
        
        var user = context.Usuarios.FirstOrDefault(u => u.nome == nome);
        
        
        if (!labComportsStudents(alocacao) || !labHasTheRequiredApps(alocacao))
        {
            alocacao.status = nameof(StatusAlocacao.Negado);
        }
        

        if (alocacao.status == nameof(StatusAlocacao.Aprovado))
        {
            alocacao.aprovado_por_id = user.id;
            alocacao.data_aprovacao = DateOnly.FromDateTime(DateTime.Now);
        }
        else
        {
            alocacao.aprovado_por_id = null;
            alocacao.data_aprovacao = null;
        }
        
        context.Alocacoes.Update(alocacao);
        context.SaveChanges();
        
        
        return Result<Alocacao>.Success(alocacao,200);
    }

    private bool IsTheDayValid(string diaFormatado)
    {
        return nomesDosDias.Contains(diaFormatado) ? true : false;
        
        
    }

    private bool DateIsFree(DateOnly date, TimeOnly startTime,TimeOnly endTime, int laboratorioId, int? alocacaoIgnorar)
    {
        return !context.Alocacoes.Any(
            al => al.laboratorio_id == laboratorioId 
                  && al.data_agendamento == date && al.status != "Negado" &&
                  (alocacaoIgnorar == null || al.id != alocacaoIgnorar)&&
                  startTime < al.horario_fim && endTime > al.horario_inicio);
    }

    private string formatDateName(string dateName)
    {
        return Regex.Replace(dateName,pattern, replacement: "").ToLower();
    }

    private bool labHasTheRequiredApps(Alocacao alocacao)
    {
        var appsrequeridos = context.DisciplinaAplicativos
            .Where(da => da.disciplina_id == alocacao.disciplina_id)
            .Select(da => da.aplicativo_id).ToList();
        
        if (!appsrequeridos.Any())
        {
            return true;
        }

        var appsnolab = context.LaboratorioAplicativos
            .Where(la => la.laboratorio_id == alocacao.laboratorio_id)
            .Select(la => la.aplicativo_id).ToList();

        
        return appsrequeridos.All(requeridos => appsnolab.Contains(requeridos));
        
    }
    
    

    private bool labComportsStudents(Alocacao alocacao)
    {
        var pcs = context.Laboratorios
            .Where(l => l.id == alocacao.laboratorio_id)
            .Select(l => l.qnt_computadores).FirstOrDefault();

        var alunos = context.Disciplinas
            .Where(d => d.id == alocacao.disciplina_id)
            .Select(d => d.alunos_matriculados).FirstOrDefault();

        if (pcs == 0)
        {
            return false;}
        var maxStudentsPerPc = 2.0;
        
        return !(((double)alunos / pcs) > maxStudentsPerPc);


    }
}