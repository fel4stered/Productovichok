﻿using System;
using System.Collections.Generic;

namespace ProductovichokBot.Data.Models;

public partial class House
{
    public int HouseId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
