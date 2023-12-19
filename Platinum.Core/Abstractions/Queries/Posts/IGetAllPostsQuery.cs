namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetAllPostsQuery<in T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
    }
}
