using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class UserAvailabilityPeriod
{
    public int Id { get; set; }

    public string PeriodName { get; set; } = null!;

    public int StartMonth { get; set; }

    public int StartDay { get; set; }

    public int EndMonth { get; set; }

    public int EndDay { get; set; }

    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;

    public virtual ICollection<UserAvailability> UserAvailabilities { get; set; } = new List<UserAvailability>();
}
