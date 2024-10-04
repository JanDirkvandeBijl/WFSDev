using System;
using System.Collections.Generic;

namespace WFSDev.Models;

public partial class DepartmentUser
{
    public int DepartmentId { get; set; }

    public string UserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
