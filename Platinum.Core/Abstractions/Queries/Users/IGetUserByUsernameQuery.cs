
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anhny010920.Core.Abstractions.Queries.Users
{
    public interface IGetUserByAuthenticationQuery<T> :
        IQueryHandler<User>, IQueryHandlerAsync<User>
    {
        string Username { get; set; }
        string PasswordHash { get; set; }
    }
}
