using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class ProjectType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<PropertyDefinition> PropertyDefinitions { get; set; } = new List<PropertyDefinition>();
}
