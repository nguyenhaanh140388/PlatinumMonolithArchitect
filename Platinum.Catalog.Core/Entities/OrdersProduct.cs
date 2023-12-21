using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class OrdersProduct
{
    public Guid Id { get; set; }

    public Guid? Orderid { get; set; }

    public Guid Productid { get; set; }

    public string Productname { get; set; } = null!;

    public decimal Productprice { get; set; }

    public decimal Finalprice { get; set; }

    public decimal Productstax { get; set; }

    public int Productqty { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<OrderProductAttribute> OrderProductAttributes { get; set; } = new List<OrderProductAttribute>();

    public virtual Product Product { get; set; } = null!;
}
