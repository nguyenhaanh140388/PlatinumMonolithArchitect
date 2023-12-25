using Platinum.Catalog.Core.Abstractions.Services;
using Platinum.Catalog.Core.Entities;
using Platinum.Catalog.Core.Features.Categories.Commands;
using Platinum.Core.Abstractions.Commands;
using Platinum.Core.Common;

namespace Platinum.Catalog.Infrastructure.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(ICommandService commandService) : base(commandService)
        {
        }

        public Task<ResponseObject<Category>> CreateCategoryHandler(CreateCategoryCommand payload)
        {
            return this.commandService.HandleAsync<CreateCategoryCommand, ResponseObject<Category>>(payload);
        }

        public Task<string> GetCategoriesNameQuery(string payload)
        {
            return this.commandService.FetchAsync<string, string>(payload);
        }
    }
}
