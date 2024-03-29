﻿using System;
using System.Collections.Generic;

namespace ProductovichokBot.Data.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public int? StreetId { get; set; }

    public int? HouseId { get; set; }

    public virtual House? House { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Street? Street { get; set; }

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
}
