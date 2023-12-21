// <copyright file="CustomAsyncResultFilter.cs" company="Platinum">
// Copyright (c) Platinum. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Platinum.WebApiApplication.Filters.ResultFilters
{
    /// <summary>
    /// LoggingActionFilter.
    /// </summary>
    /// <seealso cref="TypeFilterAttribute" />
    public sealed class CustomAsyncResultFilter : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAsyncResultFilter"/> class.
        /// </summary>
        public CustomAsyncResultFilter()
                            : base(typeof(CustomAsyncResultFilterImpl))
        {
        }

        /// <summary>
        /// CustomAsyncResultFilter.
        /// </summary>
        /// <seealso cref="IAsyncResultFilter" />
        public class CustomAsyncResultFilterImpl : IAsyncResultFilter
        {
            /// <summary>
            /// Called asynchronously before the action result.
            /// </summary>
            /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ResultExecutingContext" />.</param>
            /// <param name="next">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
            /// <returns>
            /// A <see cref="T:System.Threading.Tasks.Task" /> that on completion indicates the filter has executed.
            /// </returns>
            /// <exception cref="NotImplementedException"></exception>
            public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
            {
                // do something before the filter executes
                if (context.Result is ObjectResult result)
                {
                }

                await next();

                // do something after the filter executes
            }
        }
    }
}
