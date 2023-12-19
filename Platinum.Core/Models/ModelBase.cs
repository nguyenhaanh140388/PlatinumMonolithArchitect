using Platinum.Core.Common;

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

        /// <summary>
        /// The time stamp
        /// </summary>
        private byte[] timeStamp;

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <returns>Result.</returns>
        public byte[] GetTimeStamp()
        {
            return timeStamp;
        }

        /// <summary>
        /// Sets the time stamp.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetTimeStamp(byte[] value)
        {
            timeStamp = value;
        }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
