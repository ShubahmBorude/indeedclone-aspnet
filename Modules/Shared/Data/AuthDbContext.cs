using IndeedClone.Modules.SubModules.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.Shared.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<RegisterModel> Users { get; set; } = null!;
        public DbSet<GoogleAuthModel> UserGoogleAuths { get; set; } = null!;
        public DbSet<ResetPasswordModel> PasswordReset { get; set; } = null!;

     // # API Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         // # users table
            modelBuilder.Entity<RegisterModel>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.RefNo).IsUnique();
                entity.HasIndex(u => u.Email);
                entity.Property(u => u.Status).HasConversion<int>();
                entity.Property(u => u.Created).HasColumnType("datetime");
                entity.Property(u => u.Edited).HasColumnType("datetime");
                entity.Property(u => u.Deleted).HasColumnType("datetime").IsRequired(false);
            });

         // # user_google_auth table
            modelBuilder.Entity<GoogleAuthModel>(entity =>
            {
                entity.ToTable("user_google_auth");
                entity.HasKey(g => g.Id);
                entity.HasIndex(g => new { g.RefNo, g.Issuer, g.ProvidedId }).IsUnique();
                entity.HasIndex(g => g.UserId);
                entity.HasIndex(g => g.Email);
                entity.Property(g => g.Status).HasConversion<int>();
                entity.Property(g => g.Created).HasColumnType("datetime");
                entity.Property(g => g.Edited).HasColumnType("datetime");
                entity.Property(g => g.Deleted).HasColumnType("datetime").IsRequired(false);

             // # Relationship : Parent - users , child - user_google_auth
                entity.HasOne<RegisterModel>()
                      .WithMany()
                      .HasForeignKey(g => g.RefNo)
                      .HasPrincipalKey(u => u.RefNo)
                      .OnDelete(DeleteBehavior.Restrict);
            });

           

            base.OnModelCreating(modelBuilder);

        }

    }
}
