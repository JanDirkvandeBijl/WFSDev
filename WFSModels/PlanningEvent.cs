using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class PlanningEvent
{
    public int Id { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public int PlanningTaskId { get; set; }

    public int SortIndex { get; set; }

    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public virtual ICollection<PlanningEventAssignment> PlanningEventAssignments { get; set; } = new List<PlanningEventAssignment>();

    public virtual PlanningTask PlanningTask { get; set; } = null!;
}
