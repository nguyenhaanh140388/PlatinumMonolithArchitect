using Platinum.Catalog.Core.Entities;
using Platinum.Catalog.Core.Features.Categories.Commands;
using Platinum.Core.Common;

namespace Platinum.Catalog.Core.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<string> GetCategoriesNameQuery(string payload);

        Task<ResponseObject<Category>> CreateCategoryHandler(CreateCategoryCommand payload);

    }
}
