using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Repository
{
    public class QuantityMeasurementDbContext : DbContext
    {
        public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.OwnsOne(e => e.Operand1, o =>
                {
                    o.Property(p => p.Value).HasColumnName("Operand1Value");
                    o.Property(p => p.Unit).HasColumnName("Operand1Unit");
                });

                entity.OwnsOne(e => e.Operand2, o =>
                {
                    o.Property(p => p.Value).HasColumnName("Operand2Value");
                    o.Property(p => p.Unit).HasColumnName("Operand2Unit");
                });

                entity.OwnsOne(e => e.Result, o =>
                {
                    o.Property(p => p.Value).HasColumnName("ResultValue");
                    o.Property(p => p.Unit).HasColumnName("ResultUnit");
                });
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
