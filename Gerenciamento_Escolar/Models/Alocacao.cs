using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table(name:"alocacao")]
public class Alocacao
{
    public Alocacao( int disciplina_id, int laboratorio_id, string dia_da_semana,DateOnly data_agendamento, TimeSpan horario_inicio, TimeSpan horario_fim, string status, int? aprovado_por_id, DateOnly? data_aprovacao, int? coordenador_id)
    {
        
        this.disciplina_id = disciplina_id;
        this.laboratorio_id = laboratorio_id;
        this.dia_da_semana = dia_da_semana;
        this.data_agendamento = data_agendamento;
        this.horario_inicio = horario_inicio;
        this.horario_fim = horario_fim;
        this.status = status;
        this.aprovado_por_id = aprovado_por_id; 
        this.data_aprovacao = data_aprovacao;
        this.coordenador_id = coordenador_id;
    }

    public Alocacao(int id, int disciplina_id, int laboratorio_id, string dia_da_semana,DateOnly data_agendamento, TimeSpan horario_inicio, TimeSpan horario_fim, string status, int? aprovado_por_id, DateOnly? data_aprovacao, int? coordenador_id)
    {
        this.id = id;
        this.disciplina_id = disciplina_id;
        this.laboratorio_id = laboratorio_id;
        this.dia_da_semana = dia_da_semana;
        this.data_agendamento = data_agendamento;
        this.horario_inicio = horario_inicio;
        this.horario_fim = horario_fim;
        this.status = status;
        this.aprovado_por_id = aprovado_por_id; 
        this.data_aprovacao = data_aprovacao;
        this.coordenador_id = coordenador_id;
    }

    protected Alocacao()
    {
    }

    [Key]
    [Column(name:"id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; private set; }
    
    [Column(name:"disciplina_id")]
    [ForeignKey(name:"disciplina.id")]
    public int disciplina_id { get; set; }
    
    [Column(name:"laboratorio_id")]
    [ForeignKey(name:"laboratorio.id")]
    public int laboratorio_id { get; set; }
    
    
    [Column(name:"dia_da_semana")]
    public string dia_da_semana { get; set; }
    
    [Column(name: "data_agendamento")]
    public DateOnly data_agendamento { get; set; }
    
    [Column(name: "horario_inicio")]
    public TimeSpan horario_inicio { get; set; }
    
    [Column(name: "horario_fim")]
    public TimeSpan horario_fim { get; set; }
    
    [Column(name: "status")]
    public string status { get; set; }
    
    [Column(name: "aprovado_por_id")]
    public int? aprovado_por_id { get; set; }
    
    
    [Column(name: "data_aprovacao")]
    public DateOnly? data_aprovacao { get; set; }
    
    [Column(name: "coordenador_id")]
    public int? coordenador_id { get; set; }
}