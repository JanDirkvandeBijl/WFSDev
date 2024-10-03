using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class UserAvailability
{
    public int Id { get; set; }

    public int DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int? DepartmentId { get; set; }

    public int PeriodId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual UserAvailabilityPeriod Period { get; set; } = null!;
}
