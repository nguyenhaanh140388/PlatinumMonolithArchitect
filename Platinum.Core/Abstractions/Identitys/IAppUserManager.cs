// <copyright file="AppUserManager.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Anhny010920.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Platinum.Core.Abstractions.Identitys
{
    public interface IAppUserManager
    {
        ClaimsPrincipal CurrentUser { get; }
        Guid? CurrentUserId { get; }
        List<string> ListRoles { get; }
        string Email { get; }
        string GenerateNewAuthenticatorKey();
        Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(ApplicationUser user, int number);
        Task<ApplicationUser> GetApplicationUser();
        Task<string> GetAuthenticatorKeyAsync(ApplicationUser user);
        Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(ApplicationUser user, string code);
    }
}