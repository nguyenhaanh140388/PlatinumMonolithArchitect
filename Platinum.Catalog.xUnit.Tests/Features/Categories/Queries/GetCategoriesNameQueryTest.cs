using Platinum.Catalog.Core.Features.Categories.Queries;

namespace Platinum.Catalog.xUnit.Tests.Features.Categories.Queries
{
    public class GetCategoriesNameQueryTest
    {
        public GetCategoriesNameQueryTest()
        {
        }

        [Fact]
        public async Task GetAllDepartmentsQuery()
        {
            var query = new GetCategoriesNameQuery();
            var actual = await query.FetchAsync("Haha");

            Assert.NotNull(actual);
            Assert.Equal("HihiHaha", actual);
        }
    }
}
