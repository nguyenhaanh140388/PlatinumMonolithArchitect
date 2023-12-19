namespace Platinum.Core.Abstractions.Queries.Users
{
    public interface IGetUserByAuthenticationQuery<T> :
        IQueryHandler<User>, IQueryHandlerAsync<User>
    {
        string Username { get; set; }
        string PasswordHash { get; set; }
    }
}
