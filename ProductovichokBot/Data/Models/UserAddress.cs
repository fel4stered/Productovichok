using System;
using System.Collections.Generic;

namespace ProductovichokBot.Data.Models;

public partial class UserAddress
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public int? Appartament { get; set; }

    public int? Floor { get; set; }

    public int? Entrance { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
