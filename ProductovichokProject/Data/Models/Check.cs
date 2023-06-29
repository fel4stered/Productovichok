using System;
using System.Collections.Generic;

namespace ProductovichokProject.Data.Models;

public partial class Check
{
    public int ChecksId { get; set; }

    public int? UserId { get; set; }

    public int? OrderId { get; set; }

    public virtual Order Order { get; set; }

    public virtual User User { get; set; }
}
