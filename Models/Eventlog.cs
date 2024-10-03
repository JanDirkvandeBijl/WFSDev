using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class Eventlog
{
    public string? Message { get; set; }

    public string? MessageTemplate { get; set; }

    public int? Level { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Exception { get; set; }

    public string? LogEvent { get; set; }
}
