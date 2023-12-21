using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class ReviewsDetail
{
    public Guid Id { get; set; }

    public Guid? Reviewid { get; set; }

    public string Text { get; set; } = null!;

    public virtual Review? Review { get; set; }
}
