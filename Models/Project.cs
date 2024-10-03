using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreateDate { get; set; }

    public int TypeId { get; set; }

    public virtual ICollection<Planning> Plannings { get; set; } = new List<Planning>();

    public virtual ICollection<ProjectProperty> ProjectProperties { get; set; } = new List<ProjectProperty>();

    public virtual ProjectType Type { get; set; } = null!;
}
