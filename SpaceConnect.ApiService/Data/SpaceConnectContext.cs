using Microsoft.EntityFrameworkCore;
using SpaceConnect.ApiService.Models;

namespace SpaceConnect.ApiService.Data;

public class SpaceConnectContext : DbContext
{
    public SpaceConnectContext(DbContextOptions<SpaceConnectContext> options) : base(options) { }

    public DbSet<Tecnologia> Tecnologias => Set<Tecnologia>();
    public DbSet<CategoriaImpacto> Categorias => Set<CategoriaImpacto>();
    public DbSet<Missao> Missoes => Set<Missao>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Tecnologia>()
            .HasOne(t => t.Categoria)
            .WithMany(c => c.Tecnologias)
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tecnologia>()
            .HasOne(t => t.Missao)
            .WithMany(m => m.Tecnologias)
            .HasForeignKey(t => t.MissaoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
