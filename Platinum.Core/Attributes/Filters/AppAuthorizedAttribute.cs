// <copyright file="AuthorizeAttribute.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Platinum.Core.Enums;

namespace Platinum.Core.Attributes.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AppAuthorizedAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Roles> _roles;

        public AppAuthorizedAttribute(params Roles[] roles)
        {
            _roles = roles ?? new Roles[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUser = (ApplicationUser)context.HttpContext.Items["User"];
            var userRoles = (List<string>)context.HttpContext.Items["Roles"];
            if (currentUser == null || _roles.Any() && !_roles.Any(x => userRoles.Contains(x.ToString())))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
