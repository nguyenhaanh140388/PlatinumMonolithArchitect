namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByAuthorQuery<T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
        string Author { get; set; }
    }
}
