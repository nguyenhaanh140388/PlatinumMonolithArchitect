using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Configuration
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid? Configurationgroupid { get; set; }

    public int Order { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ConfigurationGroup? Configurationgroup { get; set; }
}
