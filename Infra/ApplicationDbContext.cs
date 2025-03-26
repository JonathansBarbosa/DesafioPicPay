using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Domain.Entities;

namespace PicPaySimplificado.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
       
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Carteira> Carteiras { get; set; } // Adicionado DbSet para Carteira
        public DbSet<Transacao> Transacoes { get; set; } // Renomeado para Transacoes

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração dos relacionamentos (Fluent API)

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.CpfCnpj)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Carteira>()
                .HasOne(c => c.Usuario)
                .WithOne() // Um usuário tem uma carteira
                .HasForeignKey<Carteira>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade); //Exclusão em cascata, quando o usuário for excluido sua carteira também será

            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Pagador)
                .WithMany() // Um usuário pode fazer várias transações como pagador
                .HasForeignKey(t => t.PagadorId)
                .OnDelete(DeleteBehavior.Restrict); // Impede exclusão em cascata

            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.Recebedor)
                .WithMany() // Um usuário pode receber várias transações
                .HasForeignKey(t => t.RecebedorId)
                .OnDelete(DeleteBehavior.Restrict); // Impede exclusão em cascata
        }
    }
}