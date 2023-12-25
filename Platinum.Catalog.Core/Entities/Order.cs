using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid? Customerid { get; set; }

    public string Customername { get; set; } = null!;

    public string CustomerStreetaddress { get; set; } = null!;

    public string Customercity { get; set; } = null!;

    public string Customerstate { get; set; } = null!;

    public string Customerpostalcode { get; set; } = null!;

    public string Customercountry { get; set; } = null!;

    public string Customertelephone { get; set; } = null!;

    public string Customeremail { get; set; } = null!;

    public string Deliveryname { get; set; } = null!;

    public string Deliverystreetaddress { get; set; } = null!;

    public string Deliverycity { get; set; } = null!;

    public string Deliverystate { get; set; } = null!;

    public string Deliverypostalcode { get; set; } = null!;

    public string Deliverycountry { get; set; } = null!;

    public Guid? Paymentmethodid { get; set; }

    public DateTime Latsmodified { get; set; }

    public DateTime Datepurcahsed { get; set; }

    public decimal? Shippingcost { get; set; }

    public Guid? Shipingmethodid { get; set; }

    public string Orderstatus { get; set; } = null!;

    public DateTime Orderdatefinished { get; set; }

    public string? Comments { get; set; }

    public string Currency { get; set; } = null!;

    public decimal CurrencyValue { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderProductAttribute> OrderProductAttributes { get; set; } = new List<OrderProductAttribute>();

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();
}
