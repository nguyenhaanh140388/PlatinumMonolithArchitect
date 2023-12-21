using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class OrderProductAttribute
{
    public Guid Id { get; set; }

    public Guid? Orderid { get; set; }

    public Guid? Orderproductid { get; set; }

    public string Productoptions { get; set; } = null!;

    public string Productoptiobvalue { get; set; } = null!;

    public decimal Optionvalueprice { get; set; }

    public string PricePrefix { get; set; } = null!;

    public virtual Order? Order { get; set; }

    public virtual OrdersProduct? Orderproduct { get; set; }
}
