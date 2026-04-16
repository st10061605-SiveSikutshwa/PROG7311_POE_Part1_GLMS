using GLMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace GLMS.Data
{
    // AppDbContext is the main EF Core class used to work with the database
    public class AppDbContext : DbContext
    {
        // Constructor receives database options from Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // These DbSet properties represent tables in the database
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One Client can have many Contracts
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Contracts)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // One Contract can have many Service Requests
            modelBuilder.Entity<Contract>()
                .HasMany(c => c.ServiceRequests)
                .WithOne(sr => sr.Contract)
                .HasForeignKey(sr => sr.ContractId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}