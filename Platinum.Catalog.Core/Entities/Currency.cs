using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Currency
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Symboleleft { get; set; }

    public string? Symbolright { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
