using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WFSDev.Models;

namespace WFSDev.Data;

public partial class ProductionDbContext : DbContext
{
    public ProductionDbContext()
    {
    }

    public ProductionDbContext(DbContextOptions<ProductionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<CalendarEvent> CalendarEvents { get; set; }

    public virtual DbSet<Culture> Cultures { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentUser> DepartmentUsers { get; set; }

    public virtual DbSet<EnvironmentSetting> EnvironmentSettings { get; set; }

    public virtual DbSet<Eventlog> Eventlogs { get; set; }

    public virtual DbSet<LocalizedResource> LocalizedResources { get; set; }

    public virtual DbSet<Planning> Plannings { get; set; }

    public virtual DbSet<PlanningEvent> PlanningEvents { get; set; }

    public virtual DbSet<PlanningEventAssignment> PlanningEventAssignments { get; set; }

    public virtual DbSet<PlanningEventRelation> PlanningEventRelations { get; set; }

    public virtual DbSet<PlanningTask> PlanningTasks { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectProperty> ProjectProperties { get; set; }

    public virtual DbSet<ProjectType> ProjectTypes { get; set; }

    public virtual DbSet<PropertyDefinition> PropertyDefinitions { get; set; }

    public virtual DbSet<UserAvailability> UserAvailabilities { get; set; }

    public virtual DbSet<UserAvailabilityPeriod> UserAvailabilityPeriods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=app-wfs-01;Port=5432;Database=WFS;User Id=WFSDev;Password=KGV687jhgf%ghsm!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.CultureId, "IX_AspNetUsers_CultureId");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.Culture).WithMany(p => p.AspNetUsers)
                .HasForeignKey(d => d.CultureId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserRole>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

            entity.HasOne(d => d.Role).WithMany().HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User).WithMany().HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<CalendarEvent>(entity =>
        {
            entity.ToTable("CalendarEvent");

            entity.HasMany(d => d.Users).WithMany(p => p.CalendarEvents)
                .UsingEntity<Dictionary<string, object>>(
                    "ApplicationUserCalendarEvent",
                    r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UsersId"),
                    l => l.HasOne<CalendarEvent>().WithMany().HasForeignKey("CalendarEventsId"),
                    j =>
                    {
                        j.HasKey("CalendarEventsId", "UsersId");
                        j.ToTable("ApplicationUserCalendarEvent");
                        j.HasIndex(new[] { "UsersId" }, "IX_ApplicationUserCalendarEvent_UsersId");
                    });
        });

        modelBuilder.Entity<Culture>(entity =>
        {
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<DepartmentUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.DepartmentId });

            entity.HasIndex(e => e.DepartmentId, "IX_DepartmentUsers_DepartmentId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Department).WithMany(p => p.DepartmentUsers).HasForeignKey(d => d.DepartmentId);

            entity.HasOne(d => d.User).WithMany(p => p.DepartmentUsers).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<EnvironmentSetting>(entity =>
        {
            entity.HasKey(e => e.Key);
        });

        modelBuilder.Entity<Eventlog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("eventlog");

            entity.Property(e => e.Exception).HasColumnName("exception");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.LogEvent)
                .HasColumnType("jsonb")
                .HasColumnName("log_event");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.MessageTemplate).HasColumnName("message_template");
            entity.Property(e => e.Timestamp)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
        });

        modelBuilder.Entity<LocalizedResource>(entity =>
        {
            entity.HasIndex(e => new { e.CultureId, e.Key }, "IX_LocalizedResources_CultureId_Key").IsUnique();

            entity.Property(e => e.Key).HasMaxLength(200);

            entity.HasOne(d => d.Culture).WithMany(p => p.LocalizedResources).HasForeignKey(d => d.CultureId);
        });

        modelBuilder.Entity<Planning>(entity =>
        {
            entity.HasIndex(e => e.CreatedBy, "IX_Plannings_CreatedBy");

            entity.HasIndex(e => e.ProjectId, "IX_Plannings_ProjectId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("'-infinity'::timestamp with time zone");
            entity.Property(e => e.ProjectId).HasDefaultValue(0);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Plannings).HasForeignKey(d => d.CreatedBy);

            entity.HasOne(d => d.Project).WithMany(p => p.Plannings)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PlanningEvent>(entity =>
        {
            entity.HasIndex(e => e.PlanningTaskId, "IX_PlanningEvents_PlanningTaskId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("'-infinity'::timestamp with time zone");
            entity.Property(e => e.SortIndex).HasDefaultValue(0);

            entity.HasOne(d => d.PlanningTask).WithMany(p => p.PlanningEvents)
                .HasForeignKey(d => d.PlanningTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PlanningEventAssignment>(entity =>
        {
            entity.HasIndex(e => e.EventId, "IX_PlanningEventAssignments_EventId");

            entity.HasIndex(e => e.UserId, "IX_PlanningEventAssignments_UserId");

            entity.HasOne(d => d.Event).WithMany(p => p.PlanningEventAssignments)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.PlanningEventAssignments).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<PlanningEventRelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PlanningTaskRelations");

            entity.HasIndex(e => e.PlanningId, "IX_PlanningEventRelations_PlanningId");

            entity.Property(e => e.PlanningId).HasDefaultValue(0);

            entity.HasOne(d => d.Planning).WithMany(p => p.PlanningEventRelations)
                .HasForeignKey(d => d.PlanningId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PlanningTask>(entity =>
        {
            entity.HasIndex(e => e.ApplicationUserId, "IX_PlanningTasks_ApplicationUserId");

            entity.HasIndex(e => e.CreatedBy, "IX_PlanningTasks_CreatedBy");

            entity.HasIndex(e => e.PlanningId, "IX_PlanningTasks_PlanningId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("'-infinity'::timestamp with time zone");
            entity.Property(e => e.SortIndex).HasDefaultValue(0);

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.PlanningTaskApplicationUsers).HasForeignKey(d => d.ApplicationUserId);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PlanningTaskCreatedByNavigations).HasForeignKey(d => d.CreatedBy);

            entity.HasOne(d => d.Planning).WithMany(p => p.PlanningTasks)
                .HasForeignKey(d => d.PlanningId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasIndex(e => e.TypeId, "IX_Projects_TypeId");

            entity.Property(e => e.Name).HasMaxLength(250);

            entity.HasOne(d => d.Type).WithMany(p => p.Projects).HasForeignKey(d => d.TypeId);
        });

        modelBuilder.Entity<ProjectProperty>(entity =>
        {
            entity.HasIndex(e => e.ProjectId, "IX_ProjectProperties_ProjectId");

            entity.HasIndex(e => e.PropertyDefinitionId, "IX_ProjectProperties_PropertyDefinitionId").IsUnique();

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectProperties).HasForeignKey(d => d.ProjectId);

            entity.HasOne(d => d.PropertyDefinition).WithOne(p => p.ProjectProperty).HasForeignKey<ProjectProperty>(d => d.PropertyDefinitionId);
        });

        modelBuilder.Entity<ProjectType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<PropertyDefinition>(entity =>
        {
            entity.HasIndex(e => e.ProjectTypeId, "IX_PropertyDefinitions_ProjectTypeId");

            entity.HasOne(d => d.ProjectType).WithMany(p => p.PropertyDefinitions).HasForeignKey(d => d.ProjectTypeId);
        });

        modelBuilder.Entity<UserAvailability>(entity =>
        {
            entity.HasIndex(e => e.DepartmentId, "IX_UserAvailabilities_DepartmentId");

            entity.HasIndex(e => e.PeriodId, "IX_UserAvailabilities_PeriodId");

            entity.Property(e => e.PeriodId).HasDefaultValue(0);

            entity.HasOne(d => d.Department).WithMany(p => p.UserAvailabilities).HasForeignKey(d => d.DepartmentId);

            entity.HasOne(d => d.Period).WithMany(p => p.UserAvailabilities).HasForeignKey(d => d.PeriodId);
        });

        modelBuilder.Entity<UserAvailabilityPeriod>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_UserAvailabilityPeriods_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.UserAvailabilityPeriods).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
