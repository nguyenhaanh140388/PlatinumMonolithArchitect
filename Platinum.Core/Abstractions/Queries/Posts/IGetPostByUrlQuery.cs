namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByUrlQuery<T> :
        IQueryHandler<Post>, IQueryHandlerAsync<Post>
    {
        string Url { get; set; }
    }
}
