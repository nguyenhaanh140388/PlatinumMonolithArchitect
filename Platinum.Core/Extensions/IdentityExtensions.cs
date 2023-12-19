// <copyright file="IdentityExtensions.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// IdentityExtensions.
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Processes the specified model state.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="modelState">State of the model.</param>
        /// <returns>True or false.</returns>
        public static bool Process(
            this IdentityResult result,
            ModelStateDictionary modelState)
        {
            foreach (IdentityError err in result.Errors
                    ?? Enumerable.Empty<IdentityError>())
            {
                modelState.AddModelError(err.Code, err.Description);
            }

            return result.Succeeded;
        }
    }
}
