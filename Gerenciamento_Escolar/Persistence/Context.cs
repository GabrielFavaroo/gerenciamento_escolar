using Gerenciamento_Escolar.Models;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Persistence;

public class Context : DbContext
{
    private DbContextOptions<Context> dbop;

    public Context(DbContextOptions<Context> dbop)
    {
        this.dbop = dbop;
    }

    public DbSet<Alocacao> Alocacoes;
    public DbSet<Laboratorio> Laboratorios;
    public DbSet<Aplicativo> Aplicativos;
    public DbSet<Disciplina> Disciplinas;
    public DbSet<Usuario> Usuarios;
    public DbSet<Disciplina_Aplicativo> DisciplinaAplicativos;
    public DbSet<Laboratorio_Aplicativo> LaboratorioAplicativos;
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Data Source=escola.db");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Disciplina_Aplicativo>()
            .HasKey(da => new { da.disciplina_id, da.aplicativo_id });

        modelBuilder.Entity<Laboratorio_Aplicativo>()
            .HasKey(la => new { la.aplicativo_id, la.laboratorio_id });
    }
}