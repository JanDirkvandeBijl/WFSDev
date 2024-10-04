using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class PropertyDefinition
{
    public int Id { get; set; }

    public string LocalizedLabel { get; set; } = null!;

    public string DataType { get; set; } = null!;

    public int ProjectTypeId { get; set; }

    public int Type { get; set; }

    public virtual ProjectProperty? ProjectProperty { get; set; }

    public virtual ProjectType ProjectType { get; set; } = null!;
}
