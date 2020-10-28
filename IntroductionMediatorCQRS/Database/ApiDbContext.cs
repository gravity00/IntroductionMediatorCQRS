using Microsoft.EntityFrameworkCore;

namespace IntroductionMediatorCQRS.Database
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductEntity>(cfg =>
            {
                cfg.HasKey(e => e.Id);
                cfg.HasAlternateKey(e => e.ExternalId);

                cfg.HasIndex(e => e.Code).IsUnique();

                cfg.Property(e => e.ExternalId)
                    .IsRequired();
                cfg.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(8);
                cfg.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
                cfg.Property(e => e.Price)
                    .IsRequired();
            });
        }
    }
}
