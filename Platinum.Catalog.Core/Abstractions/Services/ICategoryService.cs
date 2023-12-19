namespace Platinum.Catalog.Core.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<string> GetCategoriesNameQuery(string payload);
    }
}
