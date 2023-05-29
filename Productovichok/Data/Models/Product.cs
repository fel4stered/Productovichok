using System;
using System.Collections.Generic;

namespace Productovichok.Data.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    public string ProductName { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public int? Discount { get; set; }

    public bool? WeightOrVolume { get; set; }

    public bool? IsPieceOrWeighted { get; set; }

    public string ImageUrl { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
