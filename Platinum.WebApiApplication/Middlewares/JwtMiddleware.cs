// <copyright file="JwtMiddleware.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Platinum.Core.Settings;
using Platinum.Identity.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Platinum.WebApiApplication.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtMiddleware(RequestDelegate next,
            IOptions<JWTSettings> jwtSettings,
             UserManager<ApplicationUser> userManager)
        {
            _next = next;
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachAccountToContext(context, token);

            await _next(context);
        }

        private async Task AttachAccountToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidAudience = _jwtSettings.ValidAudience,
                    ValidIssuer = _jwtSettings.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    ValidateIssuerSigningKey = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "uid").Value;
                var roles = jwtToken.Claims.Where(x => x.Type == "roles")
                        .Select(x => x.Value)
                        .ToList();

                // attach account to context on successful jwt validation
                var user = await _userManager.FindByIdAsync(userId);
                context.Items["User"] = user;
                context.Items["Roles"] = await _userManager.GetRolesAsync(user);
            }
            catch
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }
}
