// <copyright file="ApplicationContext.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Core.Utils
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using System;
    using System.Security.Principal;

    /// <summary>
    /// ApplicationContext.
    /// </summary>
    public static class ApplicationContext
    {
        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private static IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// The action context accessor.
        /// </summary>
        private static IActionContextAccessor actionContextAccessor;

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static HttpContext HttpContext => httpContextAccessor.HttpContext;

        /// <summary>
        /// Gets the action context.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static ActionContext ActionContext => actionContextAccessor.ActionContext;

        /// <summary>
        /// Gets the identity.
        /// </summary>
        /// <value>
        /// The identity.
        /// </value>
        public static IIdentity Identity => HttpContext.User.Identity;

        /// <summary>
        /// Configures the specified HTTP context accessor.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        public static void Configure(IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
        {
            ApplicationContext.httpContextAccessor = httpContextAccessor;
            ApplicationContext.actionContextAccessor = actionContextAccessor;
        }

        public static object GetService(Type type)
        {
            return HttpContext.RequestServices.GetService(type);
        }
    }
}
