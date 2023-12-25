using Platinum.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace Platinum.Core.Models
{

    public class ModelBase : IAuditDateTime,
        IAuditConcurrency,
        IAuditUser
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
