using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table(name:"disciplina")]
public class Disciplina
{
    public Disciplina( string nome, int alunosMatriculados, string descricao, int coordenadorId)
    {
        
        this.nome = nome;
        alunos_matriculados = alunosMatriculados;
        this.descricao = descricao;
        coordenador_id = coordenadorId;
    }

    public Disciplina(int id, string nome, int alunosMatriculados, string descricao, int coordenadorId)
    {
        this.id = id;
        this.nome = nome;
        alunos_matriculados = alunosMatriculados;
        this.descricao = descricao;
        coordenador_id = coordenadorId;
    }

    [Key]
    [Column(name:"id")]
    private int id{ get; set; }
    
    [Column(name:"nome")]
    private string nome{ get; set; }
    
    [Column(name:"alunos_matriculados")]
    private int alunos_matriculados{ get; set; }
    
    [Column(name:"descricao")]
    private string descricao{ get; set; }
    
    [Column(name:"coordenador_id")]
    private int coordenador_id{ get; set; }

}