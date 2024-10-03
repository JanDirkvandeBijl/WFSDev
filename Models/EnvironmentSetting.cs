using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class EnvironmentSetting
{
    public string Key { get; set; } = null!;

    public string? Value { get; set; }
}
