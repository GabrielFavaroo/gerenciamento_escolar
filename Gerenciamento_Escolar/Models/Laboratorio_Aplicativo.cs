using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table("laboratorio_aplicativo")]
public class Laboratorio_Aplicativo
{
    public Laboratorio_Aplicativo(int laboratorio_id, int aplicativo_id)
    {
        this.laboratorio_id = laboratorio_id;
        this.aplicativo_id = aplicativo_id;
    }
    
    [Column("laboratorio_id")]
    [ForeignKey(nameof(laboratorio))]
    public int laboratorio_id{ get; private set; }
    public virtual Laboratorio laboratorio { get; set; }
    
    [Column("aplicativo_id")]
    [ForeignKey(nameof(aplicativo))]
    public int aplicativo_id{ get; private set; }
    public virtual Aplicativo aplicativo { get; set; }
}