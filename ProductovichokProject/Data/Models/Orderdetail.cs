using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductovichokProject.Data.Models;

public partial class Orderdetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int PriceAtOrder { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    [NotMapped]
    public int SumPrice
    {
        get
        {
            return PriceAtOrder * Quantity;
        }
    }
}
