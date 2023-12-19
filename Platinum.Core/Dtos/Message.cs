using System.Runtime.Serialization;

namespace Platinum.Core.Dtos
{
    /// <summary>
    /// Message.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    [DataContract]
    public class Message<TEntity>
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "IsSuccess")]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the return message.
        /// </summary>
        /// <value>
        /// The return message.
        /// </value>
        [DataMember(Name = "ReturnMessage")]
        public string ReturnMessage { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [DataMember(Name = "Data")]
        public TEntity Data { get; set; }
    }
}
