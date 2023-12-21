using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Review
{
    public Guid Id { get; set; }

    public Guid? Productid { get; set; }

    public Guid? Customerid { get; set; }

    public string Customername { get; set; } = null!;

    public int Rating { get; set; }

    public DateTime Addedon { get; set; }

    public DateTime Modifiedon { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<ReviewsDetail> ReviewsDetails { get; set; } = new List<ReviewsDetail>();
}
