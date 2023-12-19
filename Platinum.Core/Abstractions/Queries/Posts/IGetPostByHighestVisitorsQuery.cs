namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByHighestVisitorsQuery<T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
    }
}
