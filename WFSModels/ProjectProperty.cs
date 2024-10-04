using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class ProjectProperty
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int PropertyDefinitionId { get; set; }

    public string? Value { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual PropertyDefinition PropertyDefinition { get; set; } = null!;
}
