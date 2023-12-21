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
}
