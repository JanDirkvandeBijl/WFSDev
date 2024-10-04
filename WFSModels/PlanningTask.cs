using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class PlanningTask
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsLocked { get; set; }

    public int PlanningId { get; set; }

    public int SortIndex { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ApplicationUserId { get; set; }

    public virtual AspNetUser? ApplicationUser { get; set; }

    public virtual AspNetUser? CreatedByNavigation { get; set; }

    public virtual Planning Planning { get; set; } = null!;

    public virtual ICollection<PlanningEvent> PlanningEvents { get; set; } = new List<PlanningEvent>();
}
