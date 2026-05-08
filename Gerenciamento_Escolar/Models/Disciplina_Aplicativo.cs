using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table("disciplina_aplicativo")]
public class Disciplina_Aplicativo
{
    public Disciplina_Aplicativo(int disciplinaId, int aplicativoId)
    {
        disciplina_id = disciplinaId;
        aplicativo_id = aplicativoId;
    }
    [Column(name:"disciplina_id")]
    [ForeignKey(name:"disciplina.id")]
    private int disciplina_id{ get; set; }

    [Column(name:"aplicativo_id")]
    [ForeignKey(name:"aplicativo.id")]
    private int aplicativo_id{ get; set; }
    
}