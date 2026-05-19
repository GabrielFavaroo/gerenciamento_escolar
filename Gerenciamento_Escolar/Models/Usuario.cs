using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Models;

[Table("usuario")]
public class Usuario
{
    public Usuario(int id, string nome, string email, string senha, string tipoUsuario)
    {
        this.id = id;
        this.nome = nome;
        this.email = email;
        this.senha = senha;
        this.tipoUsuario = tipoUsuario;
    }

    public Usuario(string nome, string email, string senha, string tipoUsuario)
    {
        this.nome = nome;
        this.email = email;
        this.senha = senha;
        this.tipoUsuario = tipoUsuario;
    }

    public Usuario(string nome, string senha)
    {
        this.nome = nome;
        this.senha = senha;
    }

    protected Usuario()
    {
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