namespace Platinum.Core.Abstractions.Queries.Posts
{
    public interface IGetPostByIdQuery<T> :
        IQueryHandler<Post>, IQueryHandlerAsync<Post>
    {
        int? Id { get; set; }
    }
}
