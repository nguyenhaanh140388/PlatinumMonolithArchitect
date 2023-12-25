using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class CustomerInfo
{
    public Guid Id { get; set; }

    public Guid? Customerid { get; set; }

    public DateTime Lastlogon { get; set; }

    public int Logoncount { get; set; }

    public DateTime Accountcreatedon { get; set; }

    public DateTime Lastmodifiedon { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Customer? Customer { get; set; }
}
