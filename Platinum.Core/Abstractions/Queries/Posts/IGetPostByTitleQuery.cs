namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByTitleQuery<T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
        string Title { get; set; }
    }
}
