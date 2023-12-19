// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Platinum.Core.Abstractions.Models.Response
{
    /// <summary>
    /// TransactionResult.
    /// </summary>
    public interface ITransactionResult
    {
        /// <summary>
        /// Gets or sets the message error.
        /// </summary>
        /// <value>
        /// The message error.
        /// </value>
        string MessageError { get; set; }

        /// <summary>
        /// Gets or sets the message error.
        /// </summary>
        /// <value>
        /// The message error.
        /// </value>
        Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the EffectedRecord.
        /// </summary>
        /// <value>
        /// The message error.
        /// </value>
        int EffectedRecord { get; set; }

        /// <summary>
        /// Gets or sets the difference properties.
        /// </summary>
        /// <value>
        /// The difference properties.
        /// </value>
        Dictionary<string, string> DifferenceProperties { get; set; }
    }
}
