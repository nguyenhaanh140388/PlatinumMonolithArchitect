﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platinum.Core.Common
{
    public interface IAuditDateTime
    {
        DateTime? CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
    }

    public interface IAuditConcurrency
    {
        byte[] RowVersion { get; set; }
    }

    public interface IAuditUser
    {
        Guid? CreatedBy { get; set; }
        Guid? ModifiedBy { get; set; }
    }

    /// <summary>
    /// EntityBase.
    /// </summary>
    public abstract class EntityBase :
        IAuditDateTime,
        IAuditConcurrency,
        IAuditUser
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
