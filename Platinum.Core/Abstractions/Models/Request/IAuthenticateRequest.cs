using System.ComponentModel.DataAnnotations;

namespace Platinum.Core.Abstractions.Models.Request
{
    public interface IAuthenticateRequest
    {
        [Required]
        string FirstName { get; set; }

        [Required]
        string LastName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [Required]
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [Required]
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [remember me]; otherwise, <c>false</c>.
        /// </value>
        bool RememberMe { get; set; }

        string IpAddress { get; set; }
    }
}
