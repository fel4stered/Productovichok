﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductovichokProject.Data.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public int Price { get; set; }

    public int? Discount { get; set; }

    public int UnitId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public float VolumeOrWeight { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual Unit Unit { get; set; } = null!;

    [NotMapped]
    public int? DiscontPrice
    {
        get
        {
            if(this.Discount != null)
                return this.Price - (this.Price * this.Discount / 100);
            else 
                return this.Price;
        }
    }
}
