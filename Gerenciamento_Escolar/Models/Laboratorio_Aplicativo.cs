using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table("laboratorio_aplicativo")]
public class Laboratorio_Aplicativo
{
    public Laboratorio_Aplicativo(int laboratorioId, int aplicativoId)
    {
        laboratorio_id = laboratorioId;
        aplicativo_id = aplicativoId;
    }
    
    [Column("laboratorio_id")]
    [ForeignKey("laboratorio.id")]
    int laboratorio_id{ get; set; }
    
    [Column("aplicativo_id")]
    [ForeignKey("aplicativo.id")]
    int aplicativo_id{ get; set; }
}