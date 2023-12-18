// <copyright file="WebAPIError.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Anhny010920.Core.Exceptions
{
    /// <summary>
    ///
    /// </summary>
    public class WebAPIError
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is error; otherwise, <c>false</c>.
        /// </value>
        public bool isError { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        public string detail { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebAPIError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WebAPIError(string message)
        {
            this.message = message;
            this.isError = true;
        }
    }
}
