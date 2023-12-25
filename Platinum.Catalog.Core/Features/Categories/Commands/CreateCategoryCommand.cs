namespace Platinum.Catalog.Core.Features.Categories.Commands
{
    public class CreateCategoryCommand
    {
        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public Guid? Parentcatid { get; set; }

        public int Order { get; set; }
    }
}
