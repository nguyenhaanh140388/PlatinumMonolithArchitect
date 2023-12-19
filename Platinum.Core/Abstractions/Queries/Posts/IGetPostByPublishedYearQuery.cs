namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByPublishedYearQuery<T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
        int Year { get; set; }
    }
}
