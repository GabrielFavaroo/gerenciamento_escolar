using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table("disciplina_aplicativo")]
public class Disciplina_Aplicativo
{
    public Disciplina_Aplicativo(int disciplina_id, int aplicativo_id)
    {
        this.disciplina_id = disciplina_id;
        this.aplicativo_id = aplicativo_id;
    }
    [Column(name:"disciplina_id")]
    [ForeignKey(nameof(disciplina))]
    public int disciplina_id{ get; private set; }
    public virtual Disciplina disciplina { get; set; }

    [Column(name:"aplicativo_id")]
    [ForeignKey(nameof(aplicativo))]
    public int aplicativo_id{ get; private set; }
    public virtual Aplicativo aplicativo { get; set; }
    
}