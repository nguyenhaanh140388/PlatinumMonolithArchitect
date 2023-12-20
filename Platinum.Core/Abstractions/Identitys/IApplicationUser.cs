namespace Platinum.Identity.Core.Entities
{
    public interface IApplicationUser
    {
        Guid? CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        string FirstName { get; set; }
        bool IsDeleted { get; set; }
        string LastName { get; set; }
        Guid? ModifiedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}