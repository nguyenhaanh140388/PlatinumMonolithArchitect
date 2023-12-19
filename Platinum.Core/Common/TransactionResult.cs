// <copyright file="TransactionResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Platinum.Core.Abstractions.Models.Response;

namespace Platinum.Core.Common
{
    /// <summary>
    /// TransactionResult.
    /// </summary>
    /// <seealso cref="ITransactionResult" />
    public class TransactionResult : ITransactionResult
    {
        /// <summary>
        /// Gets or sets the EffectedRecord.
        /// </summary>
        /// <value>
        /// The message error.
        /// </value>
        public int EffectedRecord { get; set; }

        /// <summary>
        /// Gets or sets the message error.
        /// </summary>
        /// <value>
        /// The message error.
        /// </value>
        public string MessageError { get; set; }

        /// <summary>
        /// Gets or sets the message error.
        /// </summary>
        /// <value>
        /// The message error.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the difference properties.
        /// </summary>
        /// <value>
        /// The difference properties.
        /// </value>
        public Dictionary<string, string> DifferenceProperties { get; set; }
    }
}