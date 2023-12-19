namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPaginatedPostByKeywordQuery<in T> :
        IQueryHandler<IEnumerable<Post>>, IQueryHandlerAsync<IEnumerable<Post>>
    {
        string Keyword { get; set; }
        int PageNumber { get; set; }
        int PageCount { get; set; }
    }
}
