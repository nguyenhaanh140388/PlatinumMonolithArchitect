using Platinum.Core.Common;

namespace Platinum.Catalog.Core.Entities;

public partial class Category : EntityBase
{
    public Category()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public Guid? Parentcatid { get; set; }

    public int Order { get; set; }
}
