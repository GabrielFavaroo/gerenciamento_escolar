using Gerenciamento_Escolar.Models;
using Microsoft.EntityFrameworkCore;

namespace Gerenciamento_Escolar.Persistence;

public class Context : DbContext
{
    public DbSet<Alocacao> Alocacoes;
    public DbSet<Laboratorio> Laboratorios;
    public DbSet<Aplicativo> Aplicativos;
    public DbSet<Disciplina> Disciplinas;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Data Source=escola.db");
}