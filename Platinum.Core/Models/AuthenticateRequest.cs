using Anhny010920.Core.Abstractions.Models.Response;

namespace Platinum.Core.Models
{
    public class AuthenticateRequest : IAuthenticateRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string IpAddress { get; set; }
    }
}
