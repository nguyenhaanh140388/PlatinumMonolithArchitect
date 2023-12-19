﻿// <copyright file="AccountController.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Infrastructure.Controller
{
#if DEBUG
    //[AllowAnonymous]
#else
        [AutoValidateAntiforgeryToken]
#endif
    /// <summary>
    /// Orders controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Produces("application/json")]
    [Route("authentication")]
    public class AuthenticationController : BaseController
    {
        ///// <summary>
        ///// The user manager.
        ///// </summary>
        // private readonly UserManager<ApplicationUser> userManager;

        ///// <summary>
        ///// The sign in manager.
        ///// </summary>
        // private readonly SignInManager<ApplicationUser> signInManager;

        ///// <summary>
        ///// The application settings.
        ///// </summary>
        // private readonly AppSettings appSettings;

        /// <summary>
        /// AuthenticationRepository.
        /// </summary>
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// AccountController.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="optionsMonitor">The options monitor.</param>
        /// <param name="authenticationRepository">The authentication repository.</param>
        public AuthenticationController(IAuthenticationService authenticationService,
            IMapper mapper,
            ILogger logger)
           : base(mapper, logger)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// POST: api/Customers.
        /// Desciption: Inserting data into the customer table in database.
        /// </summary>
        /// <param name="authenticateRequestViewModel">The authenticate request view model.</param>
        /// <returns>
        ///   <see cref="OkObjectResult">OkObjectResult</see>.
        /// </returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AuthenticateRequest payload)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var origin = @"http://localhost:57572";
            var response = await authenticationService.Register(payload, origin);

            //if (authenticationCommand.Errors != null && authenticationCommand.Errors.Count() > 0)
            //{
            //    foreach (var error in authenticationCommand.Errors)
            //    {
            //        this.ModelState.TryAddModelError(error.Code, error.Description);
            //    }

            //    return this.BadRequest(this.ModelState);
            //}

            return this.Ok(new RegistrationResponseResult()
            {
                Success = true,
                Token = response.Token,
            });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin(AuthenticateRequest payload)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var origin = @"http://localhost:57572";
            var response = await authenticationService.RegisterAdmin(payload, origin);

            //if (authenticationCommand.Errors != null && authenticationCommand.Errors.Count() > 0)
            //{
            //    foreach (var error in authenticationCommand.Errors)
            //    {
            //        this.ModelState.TryAddModelError(error.Code, error.Description);
            //    }

            //    return this.BadRequest(this.ModelState);
            //}

            return this.Ok(new RegistrationResponseResult()
            {
                Success = true,
                Token = response.Token,
            });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] Guid userId, [FromQuery] string code)
        {
            return Ok(await authenticationService.ConfirmEmailAsync(userId, code));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            var origin = @"http://localhost:57572";
            await authenticationService.ForgotPassword(model, origin);
            return Ok();

        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {

            return Ok(await authenticationService.ResetPassword(model));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AuthenticateRequest payload)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            payload.IpAddress = GenerateIPAddress();
            var response = await authenticationService.Login(payload);

            switch (response.LoginStatus)
            {
                case LoginStatus.EmailConfirmed:
                    this.ModelState.AddModelError("message", "Email not confirmed yet.");
                    break;
                case LoginStatus.IsLockedOut:
                    this.ModelState.AddModelError("message", "AccountLocked.");
                    break;
                case LoginStatus.SignInFailed:
                    this.ModelState.AddModelError("message", "Sign In Failed.");
                    break;
                case LoginStatus.NoExist:
                    this.ModelState.AddModelError("message", "User not exist.");
                    break;
                case LoginStatus.WrongPassword:
                    this.ModelState.AddModelError("message", "Invalid credentials.");
                    break;
                default:
                    break;
            }

            if (response.LoginStatus != LoginStatus.Succeeded)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(new RegistrationResponseResult()
            {
                Success = true,
                Token = response.Token,
            });
        }
    }
}
