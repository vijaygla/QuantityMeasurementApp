using Microsoft.EntityFrameworkCore;
using QuantityService.Models;

namespace QuantityService.Repository
{
    public class QuantityDbContext : DbContext
    {
        public QuantityDbContext(DbContextOptions<QuantityDbContext> options)
            : base(options)
        {
        }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }

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
        }
    }
}
