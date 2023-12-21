using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Productsdetail
{
    public Guid Id { get; set; }

    public Guid? Productid { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Url { get; set; } = null!;

    public int Views { get; set; }

    public virtual Product? Product { get; set; }
}
