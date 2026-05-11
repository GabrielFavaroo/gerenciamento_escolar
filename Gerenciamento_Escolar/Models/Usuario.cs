using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table("usuario")]
public class Usuario
{
    public Usuario(string nome, string email, string senha, string tipoUsuario)
    {
        this.nome = nome;
        this.email = email;
        this.senha = senha;
        this.tipoUsuario = tipoUsuario;
    }
    [Key]
    [Column(name:"id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Column(name:"nome")]
    public string nome{ get; set; }
    
    [Column(name:"email")]
    public string email{ get; set; }
    
    [Column(name:"senha")]
    public string senha{ get; set; }
    
    [Column(name:"tipousuario")]
    public string tipoUsuario{ get; set; }
    
}