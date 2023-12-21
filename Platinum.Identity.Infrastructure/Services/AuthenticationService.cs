// <copyright file="AuthenticationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Platinum.Core.Abstractions.Dtos;
using Platinum.Core.Abstractions.Models.Request;
using Platinum.Core.Abstractions.Models.Response;
using Platinum.Core.Abstractions.Services;
using Platinum.Core.Common;
using Platinum.Core.Enums;
using Platinum.Core.Exceptions;
using Platinum.Core.Extensions;
using Platinum.Core.Models;
using Platinum.Core.Settings;
using Platinum.Core.Utils;
using Platinum.Identity.Core.Abstractions.Authentication;
using Platinum.Identity.Core.Entities;
using Platinum.Identity.Core.Models;
using Platinum.Identity.Infrastructure.Auth;
using Platinum.Infrastructure.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Platinum.Identity.Infrastructure.Services
{

    /// <summary>
    /// AuthenticationCommand.
    /// </summary>
    /// <seealso cref="Platinum.Dao.Queries.ModelQueryBase" />
    /// <seealso cref="Platinum.Interfaces.Authentication.IAuthenticationService" />
    /// <seealso cref="IAuthenticationService" />
    /// <seealso cref="ModelQueryBase" />
    /// <seealso cref="ViewModels.Commands.Authentication.IAuthenticationCommand{ViewModels.Commands.Authentication.AuthenticationCommand}" />
    /// <seealso cref="IAuthenticationCommand{AuthenticationCommand}" />
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// The sign in manager.
        /// </summary>
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        private readonly IMapper mapper;
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IActionContextAccessor actionContextAccessor;

        private readonly JWTSettings jwtSettings;

        private readonly IJwtFactory jwtFactory;
        /// <summary>
        /// Gets or sets the email service.
        /// </summary>
        /// <value>
        /// The email service.
        /// </value>
        private readonly IEmailService emailService;
        //private readonly ITemplateService templateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="optionsMonitor">The options monitor.</param>
        /// <param name="configuration">The configuration.</param>
        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JWTSettings> jwtSettings,
            IJwtFactory jwtFactory,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            IActionContextAccessor actionContextAccessor,
            IEmailService emailService
            //ITemplateService templateService
            )
        {
            this.mapper = mapper;
            this.emailService = emailService;
            //this.templateService = templateService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.jwtSettings = jwtSettings.Value;
            this.jwtFactory = jwtFactory;
            this.actionContextAccessor = actionContextAccessor;
        }

        /// <summary>
        /// Handles the asynchronous.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public async Task<IAuthenticateResponse> Login(IAuthenticateRequest payload)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(payload.Email);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return new AuthenticateResponse(user, null)
                    {
                        LoginStatus = LoginStatus.EmailConfirmed,
                    };
                }

                bool validPassword = await userManager.CheckPasswordAsync(user, payload.PasswordHash);

                if (validPassword)
                {
                    var result = await signInManager.PasswordSignInAsync(payload.Username, payload.PasswordHash, payload.RememberMe, true);

                    if (result.Succeeded)
                    {
                        JwtSecurityToken jwtSecurityToken = await jwtFactory.GenerateJwtToken(user);
                        var refreshToken = jwtFactory.GenerateRefreshToken(payload.IpAddress);
                        var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);
                        return new AuthenticateResponse(user, null)
                        {
                            LoginStatus = LoginStatus.Succeeded,
                            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                            RefreshToken = refreshToken.Token,
                            Roles = rolesList.ToList(),
                        };
                    }
                    else if (result.IsLockedOut)
                    {
                        return new AuthenticateResponse(user, null)
                        {
                            LoginStatus = LoginStatus.IsLockedOut,
                        };
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        return new AuthenticateResponse(user,
                                                        token: null)
                        {
                            LoginStatus = LoginStatus.SignInTwoFactor,
                        };
                    }
                    else
                    {
                        return new AuthenticateResponse(user, null)
                        {
                            LoginStatus = LoginStatus.SignInFailed,
                        };
                    }
                }
                else
                {
                    return new AuthenticateResponse(user, null)
                    {
                        LoginStatus = LoginStatus.WrongPassword,
                    };
                }
            }
            else
            {
                return new AuthenticateResponse(null)
                {
                    LoginStatus = LoginStatus.NoExist,
                };
            }
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <param name="payload">IAuthenticateRequest.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous operation.
        /// </returns>
        public async Task<IAuthenticateResponse> RegisterAdmin(AuthenticateRequest payload, string origin)
        {
            ApplicationUser userExist = await userManager.FindByEmailAsync(payload.Email);

            if (userExist != null)
            {
                bool isConfirmedMail = await userManager.IsEmailConfirmedAsync(userExist);
                if (!isConfirmedMail)
                {
                    return new AuthenticateResponse(userExist, null)
                    {
                        RegisterStatus = RegisterStatus.SignUpConfirm,
                    };
                }
            }

            ApplicationUser newUser = mapper.Map<ApplicationUser>(payload);

            IdentityResult identityResult = await userManager.CreateAsync(newUser);

            if (identityResult.Process(actionContextAccessor.ActionContext.ModelState))
            {
                identityResult = await userManager.AddPasswordAsync(newUser, payload.Password);

                if (identityResult.Process(actionContextAccessor.ActionContext.ModelState))
                {
                    if (await roleManager.RoleExistsAsync(Roles.SuperAdmin.ToString()))
                    {
                        await userManager.AddToRoleAsync(newUser, Roles.SuperAdmin.ToString());
                    }

                    if (await roleManager.RoleExistsAsync(Roles.User.ToString()))
                    {
                        await userManager.AddToRoleAsync(newUser, Roles.User.ToString());
                    }

                    //var confirmEmailUrl = await SendVerificationEmail(newUser, origin);
                    //TODO: Attach Email Service here and configure it via appsettings

                    //var tempplate = await templateService.GetTemplateByCodeAsync(EmailTemplateEnum.ConfirmRegister, new ConfirmRegister() { ConfirmEmailUrl = confirmEmailUrl });

                   // await emailService.SendAsync(new EmailRequest() { To = newUser.Email, Body = tempplate, Subject = "Confirm Registration" });

                    JwtSecurityToken jwtSecurityToken = await jwtFactory.GenerateJwtToken(newUser);

                    return new AuthenticateResponse(newUser, null)
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        RegisterStatus = RegisterStatus.SignUpConfirm,
                    };
                }
                else
                {
                    await userManager.DeleteAsync(newUser);
                    return new AuthenticateResponse(userExist, null)
                    {
                        RegisterStatus = RegisterStatus.WrongPassword,
                    };
                }
            }
            else
            {
                return new AuthenticateResponse(userExist, null)
                {
                    RegisterStatus = RegisterStatus.Errors,
                };
            }
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <param name="payload">IAuthenticateRequest.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous operation.
        /// </returns>
        public async Task<IAuthenticateResponse> Register(AuthenticateRequest payload, string origin)
        {
            ApplicationUser userExist = await userManager.FindByEmailAsync(payload.Email);

            if (userExist != null)
            {
                bool isConfirmedMail = await userManager.IsEmailConfirmedAsync(userExist);
                if (!isConfirmedMail)
                {
                    return new AuthenticateResponse(userExist, null)
                    {
                        RegisterStatus = RegisterStatus.SignUpConfirm,
                    };
                }
            }

            ApplicationUser newUser = mapper.Map<ApplicationUser>(payload);

            IdentityResult identityResult = await userManager.CreateAsync(newUser, payload.PasswordHash);

            if (identityResult.Process(actionContextAccessor.ActionContext.ModelState))
            {
                identityResult = await userManager.AddPasswordAsync(newUser, payload.PasswordHash);

                //if (identityResult.Process(actionContextAccessor.ActionContext.ModelState))
                //{
                    if (await roleManager.RoleExistsAsync(Roles.User.ToString()))
                    {
                        await userManager.AddToRoleAsync(newUser, Roles.User.ToString());
                    }

                    //var confirmEmailUrl = await SendVerificationEmail(newUser, origin);
                    //TODO: Attach Email Service here and configure it via appsettings

                    //var tempplate = await templateService.GetTemplateByCodeAsync(EmailTemplateEnum.ConfirmRegister, new ConfirmRegister() { ConfirmEmailUrl = confirmEmailUrl });

                    // await emailService.SendAsync(new EmailRequest() { To = newUser.Email, Body = tempplate, Subject = "Confirm Registration" });

                    JwtSecurityToken jwtSecurityToken = await jwtFactory.GenerateJwtToken(newUser);

                    return new AuthenticateResponse(newUser, null)
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        RegisterStatus = RegisterStatus.SignUpConfirm,
                    };
                //}
                //else
                //{
                //    await userManager.DeleteAsync(newUser);
                //    return new AuthenticateResponse(userExist, null)
                //    {
                //        RegisterStatus = RegisterStatus.WrongPassword,
                //    };
                //}
            }
            else
            {
                return new AuthenticateResponse(userExist, null)
                {
                    RegisterStatus = RegisterStatus.Errors,
                };
            }
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            var code = await userManager.GeneratePasswordResetTokenAsync(account);
            //var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            await emailService.SendAsync(emailRequest);
        }

        public async Task<ResponseObject<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
            var result = await userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new ResponseObject<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }

        }

        public async Task<ResponseObject<Guid>> ConfirmEmailAsync(Guid userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new ResponseObject<Guid>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }


        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "authentication/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
    }
}
