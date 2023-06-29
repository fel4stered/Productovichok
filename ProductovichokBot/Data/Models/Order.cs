using System;
using System.Collections.Generic;

namespace ProductovichokBot.Data.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? ClientId { get; set; }

    public int? CourierId { get; set; }

    public int? PickerId { get; set; }

    public int? StatusId { get; set; }

    public int? AddressId { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateTime? OrderDateTime { get; set; }

    public DateTime? DeliveryDateTime { get; set; }

    public string? OrderComment { get; set; }

    public int? BoxNumber { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Check> Checks { get; set; } = new List<Check>();

    public virtual User? Client { get; set; }

    public virtual User? Courier { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual User? Picker { get; set; }

    public virtual Status? Status { get; set; }
}
