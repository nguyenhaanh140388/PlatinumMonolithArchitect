using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Country
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Isocode { get; set; } = null!;

    public Guid? Currencyid { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Currency? Currency { get; set; }
}
