using System;
using System.Collections.Generic;

namespace ProductovichokBot.Data.Models;

public partial class Unit
{
    public int UnitId { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
