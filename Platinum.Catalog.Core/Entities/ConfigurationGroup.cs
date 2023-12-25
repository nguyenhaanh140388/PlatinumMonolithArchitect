using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class ConfigurationGroup
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Order { get; set; }

    public bool Active { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
}
