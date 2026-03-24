using ASC.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASC.Web
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<MasterDataKey> MasterDatakeys { get; set; }
        public virtual DbSet<MasterDataValue> MasterDataValues { get; set; }
        public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // ⚠️ QUAN TRỌNG

            builder.Entity<MasterDataKey>()
                .HasKey(c => new { c.PartitionKey, c.RowKey });

            builder.Entity<MasterDataValue>()
                .HasKey(c => new { c.PartitionKey, c.RowKey });

            builder.Entity<ServiceRequest>()
                .HasKey(c => new { c.PartitionKey, c.RowKey });
        }
    }
}