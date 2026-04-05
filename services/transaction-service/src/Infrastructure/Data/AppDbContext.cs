using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);

            var navigation = entity.Metadata.FindNavigation(nameof(Transaction.Entries));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });
        modelBuilder.Entity<TransactionEntry>(entity =>
        {
            entity.HasKey(te => te.Id);


        entity.OwnsOne(te => te.Amount, money =>
            {
                money.Property(m => m.Value)
                     .HasColumnName("Amount")
                     .HasColumnType("numeric(18,2)") 
                     .IsRequired();
            });
        });
    }
}