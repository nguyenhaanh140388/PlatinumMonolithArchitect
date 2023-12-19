// <copyright file="GenerateAntiforgeryTokenCookieAttribute.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Anhny010920WebAppApi.Filters.ResultFilters
{
    /// <summary>
    /// GenerateAntiforgeryTokenCookieAttribute.
    /// </summary>
    /// <seealso cref="ResultFilterAttribute" />
    public sealed class GenerateAntiforgeryTokenCookieAttribute : ResultFilterAttribute
    {
        /// <summary>
        /// OnResultExecuting.
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            IAntiforgery antiforgery = (IAntiforgery)context.HttpContext.RequestServices.GetService(typeof(IAntiforgery));

            // Send the request token as a JavaScript-readable cookie
            var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);

            context.HttpContext.Response.Cookies.Append(
                "RequestVerificationToken",
                tokens.RequestToken,
                new CookieOptions() { HttpOnly = false });
        }

        /// <summary>
        /// OnResultExecuted.
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public override void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
