﻿using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class ProductsOptionsValue
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<CartAttribute> CartAttributes { get; set; } = new List<CartAttribute>();

    public virtual ICollection<ProductsAttribute> ProductsAttributes { get; set; } = new List<ProductsAttribute>();

    public virtual ICollection<ProductsOptionsValuesMapping> ProductsOptionsValuesMappings { get; set; } = new List<ProductsOptionsValuesMapping>();
}
