using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WFSDev.Models;

namespace WFSDev.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        //dotnet ef migrations add InitialMigration --context ApplicationDbContext
        //dotnet ef database update --context ApplicationDbContext
        public DbSet<Client> Clients { get; set; }
        public DbSet<ConnectionString> ConnectionStrings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.Address);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasMany(e => e.ConnectionStrings)
             .WithOne(e => e.Client)
             .HasForeignKey(e => e.ClientId);
            });


            modelBuilder.Entity<ConnectionString>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ServerName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DatabaseName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.DatabaseUserId)
                    .HasMaxLength(100);

                entity.Property(e => e.DatabasePassword)
                    .HasMaxLength(100);

                entity.Property(e => e.AdditionalParameters);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            });
        }
    }
}
