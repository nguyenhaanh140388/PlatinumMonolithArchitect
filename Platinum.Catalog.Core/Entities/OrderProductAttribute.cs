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

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Order? Order { get; set; }

    public virtual OrdersProduct? Orderproduct { get; set; }
}
