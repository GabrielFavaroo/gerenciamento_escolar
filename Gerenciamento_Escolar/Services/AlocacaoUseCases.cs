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
    

    public Alocacao criar(AlocacaoDTO alocacaoDto)
    {
        
        
        if (context.Alocacoes.Any(al =>
                al.data_agendamento == alocacaoDto.dataAgendamento
                && alocacaoDto.horario_inicio > al.horario_fim || al.horario_inicio > alocacaoDto.horario_fim)
           )
        {
            throw new Exception("O horario da alocação já estava reservado");
        }

        var diaFormatado = Regex.Replace(input: alocacaoDto.dia_da_semana,pattern, replacement: "").ToLower();


        if (!nomesDosDias.Contains(diaFormatado))
        {
            throw new Exception("Dia da semana inválido");
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
        return alocacao;
    }
    public Alocacao procurarUm(int id)
    {
        
        var alocacao = context.Alocacoes.Find(id);
        if (alocacao == null)
        {
            throw new ("Alocação não encontrada");
        }
        return alocacao;
    }
    public ListaDeAlocacoesDTO listar(int pagina, int quantidade)
    {
        var alocacoes = context.Alocacoes.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();
        
        
        
        var listaEDados = new ListaDeAlocacoesDTO(pagina, quantidade, alocacoes);
        return listaEDados;
    }
    public void remover(int id)
    {

        context.Alocacoes.Where(a => a.id == id).ExecuteDelete();

    }
    
    public Alocacao atualizar(int id,AlocacaoAtualizadaDTO alocacaoAtualizadaDto)
    {   
        if(context.Alocacoes.Any(al => 
                    al.data_agendamento == alocacaoAtualizadaDto.dataAgendamento
                    && alocacaoAtualizadaDto.horario_inicio > al.horario_fim || al.horario_inicio > alocacaoAtualizadaDto.horario_fim)
                )
                {
                    throw new Exception("O horario da alocação já estava reservado");
                }

        
        var diaFormatado = Regex.Replace(input: alocacaoAtualizadaDto.dia_da_semana,pattern, replacement: "").ToLower();


        if (!nomesDosDias.Contains(diaFormatado))
        {
            throw new Exception("Dia da semana inválido");
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
        
        
        var pcs = context.Laboratorios
            .Where(l => l.id == alocacaoAtualizadaDto.laboratorio_id)
            .Select(l => l.qnt_computadores).FirstOrDefault();

        var alunos = context.Disciplinas
            .Where(d => d.id == alocacaoAtualizadaDto.disciplina_id)
            .Select(d => d.alunos_matriculados).FirstOrDefault();

        if ((alunos / pcs) > 2)
        {
            alocacaoAtualizada.status = nameof(StatusAlocacao.Negado);}

        var appsrequeridos = context.DisciplinaAplicativos
            .Where(da => da.disciplina_id == alocacaoAtualizadaDto.disciplina_id)
            .Select(da => da.aplicativo_id).ToList();
        if (!appsrequeridos.Any())
        {
            alocacaoAtualizada.status = nameof(StatusAlocacao.Aprovado);}

        var appsnolab = context.LaboratorioAplicativos
            .Where(la => la.laboratorio_id == alocacaoAtualizadaDto.laboratorio_id)
            .Select(la => la.aplicativo_id).ToList();

        bool appsestaoinstalados = appsrequeridos.All(r => appsnolab.Contains(r));
        if (!appsestaoinstalados)
        {
            alocacaoAtualizada.status = nameof(StatusAlocacao.Negado);
        }

        if (alocacaoAtualizada.status == nameof(StatusAlocacao.Aprovado))
        {
            alocacaoAtualizada.data_aprovacao = new DateTime();
        }
        
        context.Alocacoes.Update(alocacaoAtualizada);
        context.SaveChanges();
        var alocacao = context.Alocacoes.Find(id);
        
        return alocacao;
    }
    
    
    
}