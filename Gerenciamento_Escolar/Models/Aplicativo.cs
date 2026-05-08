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
    private int id{ get; set; }
    
    [Column(name:"nome")]
    private string nome{ get; set; }
    
    [Column(name:"versao")]
    private string versao{ get; set; }
    
    [Column(name:"descricao")]
    private string descricao{ get; set; }
}