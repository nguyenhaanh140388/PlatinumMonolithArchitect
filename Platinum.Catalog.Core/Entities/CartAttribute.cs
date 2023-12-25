using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class CartAttribute
{
    public Guid Id { get; set; }

    public Guid? Customerid { get; set; }

    public Guid? Productid { get; set; }

    public Guid? Productoptionid { get; set; }

    public Guid? Productoptionvalueid { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ProductsOption? Productoption { get; set; }

    public virtual ProductsOptionsValue? Productoptionvalue { get; set; }
}
