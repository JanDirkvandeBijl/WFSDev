using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DepartmentUser> DepartmentUsers { get; set; } = new List<DepartmentUser>();

    public virtual ICollection<UserAvailability> UserAvailabilities { get; set; } = new List<UserAvailability>();
}
