// <copyright file="AuthResult.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

namespace Platinum.Core.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// AuthResult.
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AuthResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<string> Errors { get; set; }
    }
}
