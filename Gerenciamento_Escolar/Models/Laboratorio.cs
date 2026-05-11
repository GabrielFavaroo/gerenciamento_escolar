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

    public Laboratorio(int id, string nome, int qnt_computadores)
    {
        this.id = id;
        this.nome = nome;
        this.qnt_computadores = qnt_computadores;
    }

    [Key]
    public int id{ get; private set; }
    
    [Column(name:"nome")]
    public string nome{ get; set; }
    
    [Column(name:"qnt_computadores")]
    public int qnt_computadores{ get; private set; }

    public virtual ICollection<Laboratorio_Aplicativo> laboratorioAplicativos { get; set; }

}