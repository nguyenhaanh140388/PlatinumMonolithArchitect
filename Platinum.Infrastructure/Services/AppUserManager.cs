// <copyright file="AppUserManager.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Platinum.Core.Abstractions.Identitys;
using Platinum.Identity.Core.Entities;
using System.Security.Claims;

namespace Platinum.Infrastructure.Services
{
    /// <summary>
    /// Custom UserManager to override Authenticator Token generation behavior (encrypt/decrypt).
    /// </summary>
    /// <seealso cref="UserManager{ApplicationUser}" />
    public class AppUserManager : UserManager<IApplicationUser>, IAppUserManager
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IConfiguration configuration;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly ILogger<UserManager<IApplicationUser>> logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUserManager" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="optionsAccessor">The options accessor.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        /// <param name="userValidators">The user validators.</param>
        /// <param name="passwordValidators">The password validators.</param>
        /// <param name="keyNormalizer">The key normalizer.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="services">The services.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public AppUserManager(
            IUserStore<IApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<IApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<IApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger,
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor
        )
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.actionContextAccessor = actionContextAccessor;
        }

        public ClaimsPrincipal CurrentUser => actionContextAccessor?.ActionContext?.HttpContext?.User;

        public Guid? CurrentUserId
        {
            get
            {
                try
                {
                    var claimGuid = CurrentUser?.Claims.Where(x => x.Type.StartsWith("uid"))
                        ?.FirstOrDefault()
                        ?.Value;
                    return string.IsNullOrEmpty(claimGuid) ? Guid.Empty : Guid.Parse(claimGuid);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                    return null;
                }
            }
        }

        public List<string> ListRoles
        {
            get
            {
                try
                {
                    var roles = CurrentUser.Claims.Where(x => x.Properties.Any(p => p.Value == "roles"))
                        .Select(x => x.Value)
                        .ToList();
                    return roles;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                    return null;
                }
            }
        }

        public string Email
        {
            get
            {
                try
                {
                    var email = CurrentUser.Claims.Where(x => x.Properties.Any(p => p.Value == "email"))
                        .Select(x => x.Value)
                        .FirstOrDefault();
                    return email;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Generates a new base32 encoded 160-bit security secret (size of SHA1 hash).
        /// </summary>
        /// <returns>
        /// The new security secret.
        /// </returns>
        public override string GenerateNewAuthenticatorKey()
        {
            var originalAuthenticatorKey = base.GenerateNewAuthenticatorKey();

            // var aesKey = EncryptProvider.CreateAesKey();
            bool.TryParse(configuration["TwoFactorAuthentication:EncryptionEnabled"], out bool encryptionEnabled);

            //var encryptedKey = encryptionEnabled
            //    ? EncryptProvider.AESEncrypt(originalAuthenticatorKey, this.configuration["TwoFactorAuthentication:EncryptionKey"])
            //    : originalAuthenticatorKey;

            return null;
        }

        /// <summary>
        /// Gets the authenticator key asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// The authenticator key.
        /// </returns>
        public override async Task<string> GetAuthenticatorKeyAsync(ApplicationUser user)
        {
            var databaseKey = await base.GetAuthenticatorKeyAsync(user);

            if (databaseKey == null)
            {
                return null;
            }

            // Decryption
            bool.TryParse(configuration["TwoFactorAuthentication:EncryptionEnabled"], out bool encryptionEnabled);

            //var originalAuthenticatorKey = encryptionEnabled
            //    ? EncryptProvider.AESDecrypt(databaseKey, this.configuration["TwoFactorAuthentication:EncryptionKey"])
            //    : databaseKey;

            return null;
        }

        /// <summary>
        /// Generate a new recovery code.
        /// </summary>
        /// <returns>Result.</returns>
        protected override string CreateTwoFactorRecoveryCode()
        {
            var originalRecoveryCode = base.CreateTwoFactorRecoveryCode();

            bool.TryParse(configuration["TwoFactorAuthentication:EncryptionEnabled"], out bool encryptionEnabled);

            //var encryptedRecoveryCode = encryptionEnabled
            //    ? EncryptProvider.AESEncrypt(originalRecoveryCode, this.configuration["TwoFactorAuthentication:EncryptionKey"])
            //    : originalRecoveryCode;

            return null;
        }

        /// <summary>
        /// Generates the new two factor recovery codes asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="number">The number.</param>
        /// <returns>
        /// The new recovery codes for the user.  Note: there may be less than number returned, as duplicates will be removed.
        /// </returns>
        public override async Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(ApplicationUser user, int number)
        {
            var tokens = await base.GenerateNewTwoFactorRecoveryCodesAsync(user, number);

            var generatedTokens = tokens as string[] ?? tokens.ToArray();
            if (!generatedTokens.Any())
            {
                return generatedTokens;
            }

            bool.TryParse(configuration["TwoFactorAuthentication:EncryptionEnabled"], out bool encryptionEnabled);

            return null;

            //encryptionEnabled
            //? generatedTokens
            //    .Select(token =>
            //        EncryptProvider.AESDecrypt(token, this.configuration["TwoFactorAuthentication:EncryptionKey"]))
            //: generatedTokens;
        }

        /// <summary>
        /// Redeems the two factor recovery code asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="code">The code.</param>
        /// <returns>
        /// True if the recovery code was found for the user.
        /// </returns>
        public override Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(ApplicationUser user, string code)
        {
            bool.TryParse(configuration["TwoFactorAuthentication:EncryptionEnabled"], out bool encryptionEnabled);

            if (encryptionEnabled && !string.IsNullOrEmpty(code))
            {
                code = null; // EncryptProvider.AESEncrypt(code, this.configuration["TwoFactorAuthentication:EncryptionKey"]);
            }

            return base.RedeemTwoFactorRecoveryCodeAsync(user, code);
        }

        public async Task<ApplicationUser> GetApplicationUser()
        {
            if (CurrentUser == null) return null;
            return await this.FindByIdAsync(CurrentUserId?.ToString());
        }
    }
}
