using Platinum.Core.Abstractions.Models.Request;

namespace Platinum.Identity.Core.Models
{
    public class AuthenticateRequest : IAuthenticateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string IpAddress { get; set; }
    }
}
