using System;
using System.Collections.Generic;

namespace Platinum.Catalog.Core.Entities;

public partial class AddressBook
{
    public Guid Id { get; set; }

    public Guid? Customerid { get; set; }

    public string Company { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Streetaddress { get; set; } = null!;

    public string Postalcode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public Guid? Countryid { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Customer? Customer { get; set; }
}
