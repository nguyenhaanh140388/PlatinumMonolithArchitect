using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class ProductsAttribute
{
    public Guid Id { get; set; }

    public Guid? Productsid { get; set; }

    public Guid? Optionsid { get; set; }

    public Guid? Optionvaluesid { get; set; }

    public decimal Price { get; set; }

    public string PricePrefix { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ProductsOption? Options { get; set; }

    public virtual ProductsOptionsValue? Optionvalues { get; set; }

    public virtual Product? Products { get; set; }
}
