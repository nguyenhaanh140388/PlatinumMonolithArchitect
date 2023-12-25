using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class AuditLog
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string OldValues { get; set; } = null!;

    public string NewValues { get; set; } = null!;

    public string AffectedColumns { get; set; } = null!;

    public string PrimaryKey { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;
}
