using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public Guid? Parentcatid { get; set; }

    public int Order { get; set; }

    public DateTime Addedon { get; set; }

    public DateTime Modifiedon { get; set; }
}
