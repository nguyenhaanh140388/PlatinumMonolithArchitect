using Microsoft.AspNetCore.Identity;

namespace Platinum.Identity.Core.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
