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

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
}
