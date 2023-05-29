using System;
using System.Collections.Generic;

namespace ProductovichokProject.Data.Models;

public partial class Code
{
    public int CodeId { get; set; }

    public int? UserId { get; set; }

    public DateTime? DateAdd { get; set; }

    public virtual User User { get; set; }
}
