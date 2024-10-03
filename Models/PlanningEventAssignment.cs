using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class PlanningEventAssignment
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public string? UserId { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public virtual PlanningEvent Event { get; set; } = null!;

    public virtual AspNetUser? User { get; set; }
}
