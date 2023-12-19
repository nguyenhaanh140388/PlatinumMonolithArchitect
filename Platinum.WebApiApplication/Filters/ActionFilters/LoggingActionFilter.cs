// <copyright file="LoggingActionFilter.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ILogger = Serilog.ILogger;

namespace Anhny010920WebAppApi.Filters.ActionFilters
{
    /// <summary>
    /// LoggingActionFilter.
    /// </summary>
    /// <seealso cref="TypeFilterAttribute" />
    public sealed class LoggingActionFilter : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingActionFilter"/> class.
        /// </summary>
        public LoggingActionFilter()
                            : base(typeof(LoggingActionFilterImpl))
        {
        }

        /// <summary>
        /// LoggingActionFilterImpl.
        /// </summary>
        /// <seealso cref="IActionFilter" />
        private class LoggingActionFilterImpl : IActionFilter
        {
            /// <summary>
            /// The logger.
            /// </summary>
            private readonly ILogger logger;

            /// <summary>
            /// Initializes a new instance of the <see cref="LoggingActionFilterImpl"/> class.
            /// </summary>
            /// <param name="logger">The logger.</param>
            public LoggingActionFilterImpl(ILogger logger)
            {
                this.logger = logger;
            }

            /// <summary>
            /// Called before the action executes, after model binding is complete.
            /// </summary>
            /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
            public void OnActionExecuting(ActionExecutingContext context)
            {
                // do something before the action executes
                logger.Information($"Action '{context.ActionDescriptor.DisplayName}' executing");
            }

            /// <summary>
            /// Called after the action executes, before the action result.
            /// </summary>
            /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext" />.</param>
            public void OnActionExecuted(ActionExecutedContext context)
            {
                // do something after the action executes
                logger.Information($"Action '{context.ActionDescriptor.DisplayName}' executed");
            }
        }
    }
}
