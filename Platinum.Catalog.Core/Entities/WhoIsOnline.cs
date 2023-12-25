using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class WhoIsOnline
{
    public Guid Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Sessionid { get; set; } = null!;

    public string Ipaddress { get; set; } = null!;

    public DateTime Timeentry { get; set; }

    public string Lastpageurl { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;
}
