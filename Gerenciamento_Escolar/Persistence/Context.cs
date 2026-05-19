using Gerenciamento_Escolar.Models;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Persistence;

public class Context : DbContext
{
    
    public Context(DbContextOptions<Context> options) : base(options){}


    public DbSet<Alocacao> Alocacoes { get; set; }
    public DbSet<Laboratorio> Laboratorios{ get; set; }
    public DbSet<Aplicativo> Aplicativos{ get; set; }
    public DbSet<Disciplina> Disciplinas{ get; set; }
    public DbSet<Usuario> Usuarios{ get; set; }
    public DbSet<Disciplina_Aplicativo> DisciplinaAplicativos{ get; set; }
    public DbSet<Laboratorio_Aplicativo> LaboratorioAplicativos{ get; set; }
 


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Disciplina_Aplicativo>()
            .HasKey(da => new { da.disciplina_id, da.aplicativo_id });

        modelBuilder.Entity<Laboratorio_Aplicativo>()
            .HasKey(la => new { la.aplicativo_id, la.laboratorio_id });
    }
}