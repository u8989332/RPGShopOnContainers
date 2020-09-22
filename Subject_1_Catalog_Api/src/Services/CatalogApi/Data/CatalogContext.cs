using CatalogApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CatalogType>(x =>
            {
                x.ToTable("CatalogType");
                x.Property(c => c.Type)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            builder.Entity<CatalogItem>(x =>
            {
                x.ToTable("Catalog");
                x.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                x.Property(c => c.Price)
                    .IsRequired();
                x.HasOne(c => c.CatalogType)
                    .WithMany()
                    .HasForeignKey(c => c.CatalogTypeId);
            });
        }

        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
    }
}
