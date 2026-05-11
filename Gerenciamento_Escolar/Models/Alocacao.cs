using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table(name:"alocacao")]
public class Alocacao
{
    public Alocacao( int disciplinaId, int laboratorioId, string diaDaSemana, TimeSpan horarioInicio, TimeSpan horarioFim, string status, int aprovadoPorId, DateTime dataAprovacao, int coordenadorId)
    {
        
        disciplina_id = disciplinaId;
        laboratorio_id = laboratorioId;
        dia_da_semana = diaDaSemana;
        horario_inicio = horarioInicio;
        horario_fim = horarioFim;
        this.status = status;
        aprovado_por_id = aprovadoPorId;
        data_aprovacao = dataAprovacao;
        coordenador_id = coordenadorId;
    }

    public Alocacao(int id, int disciplinaId, int laboratorioId, string diaDaSemana, TimeSpan horarioInicio, TimeSpan horarioFim, string status, int aprovadoPorId, DateTime dataAprovacao, int coordenadorId)
    {
        this.id = id;
        disciplina_id = disciplinaId;
        laboratorio_id = laboratorioId;
        dia_da_semana = diaDaSemana;
        horario_inicio = horarioInicio;
        horario_fim = horarioFim;
        this.status = status;
        aprovado_por_id = aprovadoPorId;
        data_aprovacao = dataAprovacao;
        coordenador_id = coordenadorId;
    }

    [Key]
    [Column(name:"id")]
    public int id { get; private set; }
    
    [Column(name:"disciplina_id")]
    [ForeignKey(name:"disciplina.id")]
    int disciplina_id { get; set; }
    
    [Column(name:"laboratorio_id")]
    [ForeignKey(name:"laboratorio.id")]
    int laboratorio_id { get; set; }
    
    
    [Column(name:"dia_da_semana")]
    string dia_da_semana { get; set; }
    
    [Column(name: "horario_inicio")]
    TimeSpan horario_inicio { get; set; }
    
    [Column(name: "horario_fim")]
    TimeSpan horario_fim { get; set; }
    
    [Column(name: "status")]
    public string status { get; set; }
    
    [Column(name: "aprovado_por_id")]
    int aprovado_por_id { get; set; }
    
    [Column(name: "data_aprovacao")]
    DateTime data_aprovacao { get; set; }
    
    [Column(name: "data_aprovacao")]
    int coordenador_id { get; set; }
}