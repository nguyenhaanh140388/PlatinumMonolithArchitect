// <copyright file="IAuthenticationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Platinum.Core.Abstractions.Commands;
using Platinum.Core.Abstractions.Dtos;
using Platinum.Core.Abstractions.Models.Request;
using Platinum.Core.Abstractions.Models.Response;
using Platinum.Core.Common;
using Platinum.Core.Models;

namespace Platinum.Core.Abstractions.Authentication
{
    /// <summary>
    /// IAuthenticationCommand.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return.</typeparam>
    /// <seealso cref="ICommandHandler{int}" />
    /// <seealso cref="ICommandHandlerAsync{int}" />
    public interface IAuthenticationService
    {
        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IAuthenticateResponse> Register(AuthenticateRequest payload, string origin);

        /// <summary>
        /// Logins this instance.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IAuthenticateResponse> Login(IAuthenticateRequest payload);
        Task<IAuthenticateResponse> RegisterAdmin(AuthenticateRequest payload, string origin);
        Task<ResponseObject<Guid>> ConfirmEmailAsync(Guid userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<ResponseObject<string>> ResetPassword(ResetPasswordRequest model);
    }
}