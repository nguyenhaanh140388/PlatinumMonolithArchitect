// <copyright file="IEmailSender.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Core.Abstractions.Communications
{
    /// <summary>
    /// IEmailSender.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        void SendMessage(ApplicationUser user, string subject, params string[] body);
    }
}
