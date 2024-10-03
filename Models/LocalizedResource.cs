using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFSDev.Models;

public partial class LocalizedResource
{
    public int Id { get; set; }

    public int CultureId { get; set; }

    public string Key { get; set; } = null!;

    public string Translation { get; set; } = null!;
    [NotMapped]
    public virtual Culture? Culture { get; set; } 
}
