using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.JobPost.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.Shared.Data
{
    public class IndeedCloneDbContext : DbContext
    {
        public IndeedCloneDbContext(DbContextOptions<IndeedCloneDbContext> options) : base(options) { }

        // Parents only
        public DbSet<RegisterModel> Users { get; set; }
        public DbSet<JobOrganizationModel> JobOrganizations { get; set; }
        public DbSet<AuthDbContext> Auth { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         // # Users
            modelBuilder.Entity<RegisterModel>() .HasKey(u => u.Id);
            modelBuilder.Entity<RegisterModel>().HasIndex(u => u.RefNo).IsUnique();

         // # JobOrganization
            modelBuilder.Entity<JobOrganizationModel>().HasKey(j => j.Id);
            modelBuilder.Entity<JobOrganizationModel>().HasIndex(j => j.JobUid).IsUnique();

         // # FK relationship: JobOrganization → Users
            modelBuilder.Entity<JobOrganizationModel>()
                .HasOne<RegisterModel>()
                .WithMany()
                .HasForeignKey(j => j.RefNo)
                .HasPrincipalKey(u => u.RefNo)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
