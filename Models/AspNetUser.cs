using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public int? CultureId { get; set; }

    public int? DayOfWeekId { get; set; }

    public int? CalendarWeekRuleId { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual Culture? Culture { get; set; }

    public virtual ICollection<DepartmentUser> DepartmentUsers { get; set; } = new List<DepartmentUser>();

    public virtual ICollection<PlanningEventAssignment> PlanningEventAssignments { get; set; } = new List<PlanningEventAssignment>();

    public virtual ICollection<PlanningTask> PlanningTaskApplicationUsers { get; set; } = new List<PlanningTask>();

    public virtual ICollection<PlanningTask> PlanningTaskCreatedByNavigations { get; set; } = new List<PlanningTask>();

    public virtual ICollection<Planning> Plannings { get; set; } = new List<Planning>();

    public virtual ICollection<UserAvailabilityPeriod> UserAvailabilityPeriods { get; set; } = new List<UserAvailabilityPeriod>();

    public virtual ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
}
