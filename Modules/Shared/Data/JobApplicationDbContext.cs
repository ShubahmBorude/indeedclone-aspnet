using IndeedClone.Modules.SubModules.JobApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.Shared.Data
{
    public class JobApplicationDbContext : DbContext
    {
        public JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<JobApplicationModel> JobApplications { get; set; }
        public DbSet<JobApplicationCVModel> JobApplicationCVs { get; set; }
        public DbSet<RelExperienceModel> RelExperiences { get; set; }
        public DbSet<ScreenerQuestionsModel> screenerQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         // # Parent
            modelBuilder.Entity<JobApplicationModel>(entity =>
            {
                entity.ToTable("jobapplications_core");
                entity.HasKey(x => x.Id);
                entity.HasIndex(e => e.ApplicationUid).IsUnique();

             // # Unique constraint: One user cannot apply to same job multiple times
                entity.HasIndex(x => new { x.RefNo, x.JobUid }).IsUnique();
            });

            modelBuilder.Entity<JobApplicationCVModel>(entity =>
            {
                entity.ToTable("jobapplication_cv");
                entity.HasKey(x => x.Id);
                entity.HasIndex(e => e.ApplicationUid).IsUnique();
            });

            modelBuilder.Entity<RelExperienceModel>(entity =>
            {
                entity.ToTable("jobapplication_relevant_experience");
                entity.HasKey(x => x.Id);
                entity.HasIndex(e => e.ApplicationUid).IsUnique();
            });

            modelBuilder.Entity<ScreenerQuestionsModel>(entity =>
            {
                entity.ToTable("jobapplication_screener_questions");
                entity.HasKey(x => x.Id);
                entity.HasIndex(e => e.ApplicationUid).IsUnique();
            });


         // # One-to-One (Application -> CV)
            modelBuilder.Entity<JobApplicationCVModel>()
                .HasOne<JobApplicationModel>()
                .WithOne()
                .HasForeignKey<JobApplicationCVModel>(x => x.ApplicationUid)
                .HasPrincipalKey<JobApplicationModel>(x => x.ApplicationUid);

         // # One-to-One (Application -> Experience)
            modelBuilder.Entity<RelExperienceModel>()
               .HasOne<JobApplicationModel>()
               .WithOne()
               .HasForeignKey<RelExperienceModel>(x => x.ApplicationUid)
               .HasPrincipalKey<JobApplicationModel>(x => x.ApplicationUid);

         // # One-to-One (Application -> Screener Questions)
            modelBuilder.Entity<ScreenerQuestionsModel>()
               .HasOne<JobApplicationModel>()
               .WithOne()
               .HasForeignKey<ScreenerQuestionsModel>(x => x.ApplicationUid)
               .HasPrincipalKey<JobApplicationModel>(x => x.ApplicationUid);


        }

       
    }
}
