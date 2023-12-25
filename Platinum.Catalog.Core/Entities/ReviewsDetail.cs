using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class ReviewsDetail
{
    public Guid Id { get; set; }

    public Guid? Reviewid { get; set; }

    public string Text { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Review? Review { get; set; }
}
