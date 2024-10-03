using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class PlanningEventRelation
{
    public int Id { get; set; }

    public int SourceEventId { get; set; }

    public int TargetEventId { get; set; }

    public int PlanningId { get; set; }

    public virtual Planning Planning { get; set; } = null!;
}
