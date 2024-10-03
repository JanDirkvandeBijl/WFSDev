using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class Planning
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProjectId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? DeadLine { get; set; }

    public string? Description { get; set; }

    public virtual AspNetUser? CreatedByNavigation { get; set; }

    public virtual ICollection<PlanningEventRelation> PlanningEventRelations { get; set; } = new List<PlanningEventRelation>();

    public virtual ICollection<PlanningTask> PlanningTasks { get; set; } = new List<PlanningTask>();

    public virtual Project Project { get; set; } = null!;
}
