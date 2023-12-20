using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Platinum.Core.Abstractions.Identitys
{
    public interface IApplicationUserManager
    {
        ClaimsPrincipal CurrentUser { get; }
        Guid? CurrentUserId { get; }
        string Email { get; }
        List<string> ListRoles { get; }
        Task<IdentityUser> GetApplicationUser(Guid userID);
    }
}
