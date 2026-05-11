using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table("aplicativo")]
public class Aplicativo
{
    public Aplicativo(string nome, string versao, string descricao)
    {
        
        this.nome = nome;
        this.versao = versao;
        this.descricao = descricao;
    }

    public Aplicativo(int id, string nome, string versao, string descricao)
    {
        this.id = id;
        this.nome = nome;
        this.versao = versao;
        this.descricao = descricao;
    }

    [Key]
    [Column(name:"id")]
    public int id{ get; set; }
    
    [Column(name:"nome")]
    public string nome{ get; set; }
    
    [Column(name:"versao")]
    public string versao{ get; set; }
    
    [Column(name:"descricao")]
    public string descricao{ get; set; }
    
    public virtual ICollection<Disciplina_Aplicativo> disciplinaAplicativos { get; set; }
    public virtual ICollection<Laboratorio_Aplicativo> laboratorioAplicativos { get; set; }
}