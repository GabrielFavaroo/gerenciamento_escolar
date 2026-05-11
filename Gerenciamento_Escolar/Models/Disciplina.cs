using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table(name:"disciplina")]
public class Disciplina
{
    public Disciplina( string nome, int alunos_matriculados, string descricao, int coordenador_id)
    {
        
        this.nome = nome;
        this.alunos_matriculados = alunos_matriculados;
        this.descricao = descricao;
        this.coordenador_id = coordenador_id;
    }

    public Disciplina(int id, string nome, int alunos_matriculados, string descricao, int coordenador_id)
    {
        this.id = id;
        this.nome = nome;
        this.alunos_matriculados= alunos_matriculados;
        this.descricao = descricao;
        this.coordenador_id = coordenador_id;
    }

    [Key]
    [Column(name:"id")]
    public int id{ get; private set; }
    
    [Column(name:"nome")]
    public string nome{ get; set; }
    
    [Column(name:"alunos_matriculados")]
    public int alunos_matriculados{ get;  set; }
    
    [Column(name:"descricao")]
    public string descricao{ get; set; }
    
    [Column(name:"coordenador_id")]
    public int coordenador_id{ get; set; }
    
    public virtual ICollection<Disciplina_Aplicativo> disciplinaAplicativos { get; set; }

}