namespace Platinum.Core.Abstractions.Queries.Files
{
    public interface IGetFileByIdQuery<T> :
        IQueryHandler<File>, IQueryHandlerAsync<File>
    {
        Guid? Id { get; set; }
    }
}
