using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Country
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Isocode { get; set; } = null!;

    public Guid? Currencyid { get; set; }

    public virtual Currency? Currency { get; set; }
}
