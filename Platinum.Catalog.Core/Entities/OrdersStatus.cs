using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class OrdersStatus
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
}
