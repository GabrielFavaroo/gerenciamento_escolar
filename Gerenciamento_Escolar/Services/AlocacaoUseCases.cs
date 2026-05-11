using System.Collections;
using Gerenciamento_Escolar.Dtos;
using Gerenciamento_Escolar.Models;
using Gerenciamento_Escolar.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Services;

public class AlocacaoUseCases(Context context)
{
    

    public Alocacao criar(AlocacaoDTO alocacaoDto)
    {
        
        
        var alocacao = new Alocacao(alocacaoDto.disciplina_id,
            alocacaoDto.laboratorio_id,
            alocacaoDto.dia_da_semana,
            alocacaoDto.horario_inicio,
            alocacaoDto.horario_fim,
            nameof(StatusAlocacao.Pendente),
            0,
            new DateTime(0,0,0),
            0
            );
        context.Alocacoes.Add(alocacao);
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
        
        context.Alocacoes.Remove(procurarUm(id));
        
    }
    
    public Alocacao atualizar(int id,AlocacaoAtualizadaDTO alocacaoAtualizadaDto)
    {

        var alocacaoAtualizada = new Alocacao(id, alocacaoAtualizadaDto.disciplina_id,
            alocacaoAtualizadaDto.laboratorio_id, alocacaoAtualizadaDto.dia_da_semana,
            alocacaoAtualizadaDto.horario_inicio, alocacaoAtualizadaDto.horario_fim,
            alocacaoAtualizadaDto.status,
            alocacaoAtualizadaDto.aprovadoPorId,
            alocacaoAtualizadaDto.dataAprovacao, alocacaoAtualizadaDto.coordenadorId);

        var pcs = context.Laboratorios
            .Where(l => l.id == alocacaoAtualizadaDto.laboratorio_id)
            .Select(l => l.qnt_computadores).FirstOrDefault();

        var alunos = context.Disciplinas
            .Where(d => d.id == alocacaoAtualizadaDto.disciplina_id)
            .Select(d => d.alunos_matriculados).FirstOrDefault();

        if ((alunos + pcs) > 2)
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
        
        context.Alocacoes.Update(alocacaoAtualizada);
        var alocacao = context.Alocacoes.Find(id);
        return alocacao;
    }
    
}