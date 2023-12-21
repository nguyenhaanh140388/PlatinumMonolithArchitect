using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public int Qty { get; set; }

    public string Model { get; set; } = null!;

    public string Image { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime Addedon { get; set; }

    public DateTime Modifiedon { get; set; }

    public decimal Weight { get; set; }

    public byte Status { get; set; }

    public Guid? ManufactureId { get; set; }

    public Guid? Taxclassid { get; set; }

    public virtual ICollection<CartAttribute> CartAttributes { get; set; } = new List<CartAttribute>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrdersProduct> OrdersProducts { get; set; } = new List<OrdersProduct>();

    public virtual ICollection<ProductsAttribute> ProductsAttributes { get; set; } = new List<ProductsAttribute>();

    public virtual ICollection<Productsdetail> Productsdetails { get; set; } = new List<Productsdetail>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
