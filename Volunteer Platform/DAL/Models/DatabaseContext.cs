#nullable disable
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectSkill> ProjectSkills { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.ToTable("Application");

            entity.HasIndex(e => e.ProjectId, "idx_Application_ProjectId");

            entity.HasIndex(e => e.UserId, "idx_Application_UserId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("date");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValueSql("('Pending')");

            entity.HasOne(d => d.Project).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_Application_Project");

            entity.HasOne(d => d.User).WithMany(p => p.Applications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_Application_User");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.HasIndex(e => e.FileName, "UQ_Image_FileName").IsUnique();

            entity.Property(e => e.ContentType).IsRequired();
            entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(1000);
            entity.Property(e => e.FilePath).IsRequired();
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Log");

            entity.Property(e => e.Level)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");

            entity.HasIndex(e => e.TypeId, "idx_Project_TypeId");

            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Image).WithMany(p => p.Projects)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Project_Image");

            entity.HasOne(d => d.Type).WithMany(p => p.Projects)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_Project_Type");
        });

        modelBuilder.Entity<ProjectSkill>(entity =>
        {
            entity.ToTable("ProjectSkill");

            entity.HasIndex(e => e.ProjectId, "idx_ProjectSkill_ProjectId");

            entity.HasIndex(e => e.SkillId, "idx_ProjectSkill_SkillId");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectSkills)
                .HasForeignKey(d => d.ProjectId)
				 .OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_ProjectSkill_Project");

            entity.HasOne(d => d.Skill).WithMany(p => p.ProjectSkills)
				  .OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_ProjectSkill_Skill");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("Skill");

            entity.HasIndex(e => e.Name, "UQ_Skill_Name").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.ToTable("Type");

            entity.HasIndex(e => e.Name, "UQ_Type_Name").IsUnique();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ_User_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_User_Username").IsUnique();

            entity.HasIndex(e => e.Email, "idx_User_Email");

            entity.HasIndex(e => e.Username, "idx_User_Username");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(256);
            entity.Property(e => e.PwdHash)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.PwdSalt)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.SecurityToken)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}