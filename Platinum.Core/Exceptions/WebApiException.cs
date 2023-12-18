// <copyright file="WebApiException.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System;
using System.Net;

namespace Platinum.Core.Exceptions
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Exception" />
    public class WebApiException : Exception
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="statusCode">The status code.</param>
        public WebApiException(
            string message,
                            HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiException"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="statusCode">The status code.</param>
        public WebApiException(Exception ex, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(ex.Message)
        {
            StatusCode = statusCode;
        }
    }
}
