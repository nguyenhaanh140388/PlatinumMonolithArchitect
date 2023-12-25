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

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public virtual ICollection<ReviewsDetail> ReviewsDetails { get; set; } = new List<ReviewsDetail>();
}
