// <copyright file="ISMSSender.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Anhny010920.Core.Domain.Identity;
using Anhny010920.Core.Entities.Admin;

namespace Platinum.Core.Abstractions.SignInTwoFactor
{
    /// <summary>
    /// ISMSSender.
    /// </summary>
    public interface ISMSSender
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="body">The body.</param>
        void SendMessage(ApplicationUser user, params string[] body);
    }
}
