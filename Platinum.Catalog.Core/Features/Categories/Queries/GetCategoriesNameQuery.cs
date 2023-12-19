using Platinum.Core.Abstractions.Queries;
using Platinum.Core.Common;

namespace Platinum.Catalog.Core.Features.Categories.Queries
{
    public class GetCategoriesNameQuery : BaseHandler, IQueryHandlerAsync<string, string>
    {
        public async Task<string> FetchAsync(string payload)
        {
            return await Task.FromResult("Hihi" + payload);//This will compile
        }
    }
}
