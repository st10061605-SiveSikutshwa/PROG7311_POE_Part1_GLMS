using GLMS.Models;
using Microsoft.EntityFrameworkCore;
using ContractEntity = GLMS.Models.Contract;

namespace GLMS.Data
{
    // This class manages the connection between the app and the database
    public class AppDbContext : DbContext
    {
        // Constructor receives the options from Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // These properties become tables in the database
        public DbSet<Client> Clients { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One client can have many contracts
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Contracts)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // One contract can have many service requests
            modelBuilder.Entity<ContractEntity>()
                .HasMany(c => c.ServiceRequests)
                .WithOne(sr => sr.Contract)
                .HasForeignKey(sr => sr.ContractId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}