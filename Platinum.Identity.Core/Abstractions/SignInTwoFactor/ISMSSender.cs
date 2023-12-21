// <copyright file="ISMSSender.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Platinum.Identity.Core.Entities;

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
