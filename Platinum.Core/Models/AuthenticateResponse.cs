// <copyright file="AuthenticateResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Anhny010920.Core.Abstractions.Models.Response;
using Anhny010920.Core.Domain.Identity;
using Anhny010920.Core.Enums;
using System;
using System.Collections.Generic;

namespace Platinum.Core.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticateResponse : IAuthenticateResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public LoginStatus LoginStatus { get; set; }
        public List<string> Roles { get; set; }
        public RegisterStatus RegisterStatus { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateResponse"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="token">The token.</param>
        public AuthenticateResponse(ApplicationUser user, string token)
        {
            Id = user.Id;

            // this.FirstName = user.;
            // this.LastName = user.Lastname;
            Username = user.UserName;
            Token = token;
        }

        public AuthenticateResponse(string token)
        {
            Token = token;
        }
    }
}
