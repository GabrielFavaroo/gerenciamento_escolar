using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciamento_Escolar.Models;

[Table(name:"laboratorio")]
public class Laboratorio
{
    public Laboratorio( string nome, int qntComputadores)
    {
        
        this.nome = nome;
        qnt_computadores = qntComputadores;
    }

    public Laboratorio(int id, string nome, int qntComputadores)
    {
        this.id = id;
        this.nome = nome;
        qnt_computadores = qntComputadores;
    }

    [Key]
    private int id{ get; set; }
    
    [Column(name:"nome")]
    private string nome{ get; set; }
    
    [Column(name:"qnt_computadores")]
    private int qnt_computadores{ get; set; }
    
}