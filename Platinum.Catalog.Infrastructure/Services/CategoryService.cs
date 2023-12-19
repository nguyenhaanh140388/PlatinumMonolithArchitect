using Platinum.Catalog.Core.Abstractions.Services;
using Platinum.Core.Abstractions.Commands;
using Platinum.Core.Common;

namespace Platinum.Catalog.Infrastructure.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(ICommandService commandService) : base(commandService)
        {
        }

        public Task<string> GetCategoriesNameQuery(string payload)
        {
            return this.commandService.FetchAsync<string, string>(payload);
        }
    }
}
