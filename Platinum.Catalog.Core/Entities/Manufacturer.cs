using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Manufacturer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<ManufacturersInfo> ManufacturersInfos { get; set; } = new List<ManufacturersInfo>();
}
