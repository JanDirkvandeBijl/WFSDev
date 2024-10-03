using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class Culture
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();

    public virtual ICollection<LocalizedResource> LocalizedResources { get; set; } = new List<LocalizedResource>();
}
