using Microsoft.AspNetCore.Identity;

namespace Platinum.Identity.Core.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string ResetToken { get; set; }
        //public DateTime? ResetTokenExpires { get; set; }
        //public DateTime? PasswordReset { get; set; }

        // public List<RefreshToken> RefreshTokens { get; set; }
        //public bool OwnsToken(string token)
        //{
        //    return RefreshTokens?.Find(x => x.Token == token) != null;
        //}

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
