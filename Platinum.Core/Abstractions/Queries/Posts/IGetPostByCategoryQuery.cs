namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByCategoryQuery<T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
        string Category { get; set; }
    }
}
