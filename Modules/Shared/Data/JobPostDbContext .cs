using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.JobPost.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IndeedClone.Modules.Shared.Data
{
    public class JobPostDbContext: DbContext
    {
        public JobPostDbContext(DbContextOptions<JobPostDbContext> options): base(options) 
        {

        }

        public DbSet<JobOrganizationModel> JobOrganizations { get; set; }
        public DbSet<JobBasicsModel> JobBasics { get; set; }
        public DbSet<JobDetailsModel> JobDetails { get; set; }
        public DbSet<JobPayBenefitsModel> JobPayBenifits { get; set; }
        public DbSet<JobDescriptionModel> JobDescriptions { get; set; }
        public DbSet<JobPreferencesModel> JobPreferences { get; set; }
        public DbSet<JobQualificationsModel> JobQualifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         // # Parent 
            modelBuilder.Entity<JobOrganizationModel>(entity =>
            {
                entity.ToTable("job_organization");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.JobUid).IsUnique();
                entity.HasIndex(e => e.RefNo);
            });

         // # Childs
            modelBuilder.Entity<JobBasicsModel>(entity =>
            {
                entity.ToTable("job_basics");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.JobUid).IsUnique();
            });

            modelBuilder.Entity<JobDetailsModel>(entity =>
            {
                entity.ToTable("job_details");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.JobUid).IsUnique();
            });

            modelBuilder.Entity<JobPayBenefitsModel>(entity =>
            {
                entity.ToTable("job_pay_benefits");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.JobUid).IsUnique();
            });

            modelBuilder.Entity<JobDescriptionModel>(entity =>
            {
                entity.ToTable("job_description");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.JobUid).IsUnique();
            });

            modelBuilder.Entity<JobPreferencesModel>(entity => 
            {
                entity.ToTable("job_preferences");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.JobUid).IsUnique();
            });

            modelBuilder.Entity<JobQualificationsModel>(entity => 
            {
                entity.ToTable("job_qualifications");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.JobUid).IsUnique();
            });


         // # Relationship
            modelBuilder.Entity<JobOrganizationModel>()
                 .HasOne<JobBasicsModel>()
                 .WithOne()
                 .HasForeignKey<JobBasicsModel>(b => b.JobUid)
                 .HasPrincipalKey<JobOrganizationModel>(o => o.JobUid) 
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobOrganizationModel>()
                .HasOne<JobDetailsModel>()
                .WithOne()
                .HasForeignKey<JobDetailsModel>(d => d.JobUid)
                .HasPrincipalKey<JobOrganizationModel>(o => o.JobUid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobOrganizationModel>()
                .HasOne<JobPayBenefitsModel>()
                .WithOne()
                .HasForeignKey<JobPayBenefitsModel>(p => p.JobUid)
                .HasPrincipalKey<JobOrganizationModel>(o => o.JobUid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobOrganizationModel>()
                .HasOne<JobDescriptionModel>()
                .WithOne()
                .HasForeignKey<JobDescriptionModel>(de => de.JobUid)
                .HasPrincipalKey<JobOrganizationModel>(o => o.JobUid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobOrganizationModel>()
                .HasOne<JobPreferencesModel>()
                .WithOne()
                .HasForeignKey<JobPreferencesModel>(pr => pr.JobUid)
                .HasPrincipalKey<JobOrganizationModel>(o => o.JobUid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobOrganizationModel>()
                .HasOne<JobQualificationsModel>()
                .WithOne()
                .HasForeignKey<JobQualificationsModel>(q => q.JobUid)
                .HasPrincipalKey<JobOrganizationModel>(o => o.JobUid)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
