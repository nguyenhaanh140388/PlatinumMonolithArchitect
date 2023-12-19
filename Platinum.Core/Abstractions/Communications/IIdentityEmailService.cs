// <copyright file="IIdentityEmailService.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Core.Abstractions.Communications
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// IIdentityEmailService.
    /// </summary>
    public interface IIdentityEmailService
    {
        /// <summary>
        /// Gets or sets the context accessor.
        /// </summary>
        /// <value>
        /// The context accessor.
        /// </value>
        IHttpContextAccessor ContextAccessor { get; set; }

        /// <summary>
        /// Gets or sets the email sender.
        /// </summary>
        /// <value>
        /// The email sender.
        /// </value>
        IEmailSender EmailSender { get; set; }

        /// <summary>
        /// Gets or sets the link generator.
        /// </summary>
        /// <value>
        /// The link generator.
        /// </value>
        LinkGenerator LinkGenerator { get; set; }

        /// <summary>
        /// Gets or sets the token encoder.
        /// </summary>
        /// <value>
        /// The token encoder.
        /// </value>
        ITokenUrlEncoderService TokenEncoder { get; set; }

        /// <summary>
        /// Gets or sets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        UserManager<ApplicationUser> UserManager { get; set; }

        /// <summary>
        /// Sends the account confirm email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="confirmationPage">The confirmation page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendAccountConfirmEmail(ApplicationUser user, string confirmationPage);

        /// <summary>
        /// Sends the password recovery email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="confirmationPage">The confirmation page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendPasswordRecoveryEmail(ApplicationUser user, string confirmationPage);
    }
}