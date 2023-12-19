using System.ComponentModel.DataAnnotations;

namespace Platinum.Core.Abstractions.Dtos
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
