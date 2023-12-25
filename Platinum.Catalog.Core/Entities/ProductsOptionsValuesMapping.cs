using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class ProductsOptionsValuesMapping
{
    public Guid Id { get; set; }

    public Guid? Optionsid { get; set; }

    public Guid? Valuesid { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ProductsOption? Options { get; set; }

    public virtual ProductsOptionsValue? Values { get; set; }
}
