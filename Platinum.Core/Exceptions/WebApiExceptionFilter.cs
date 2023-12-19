// <copyright file="WebApiExceptionFilter.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using System.Net;

namespace Platinum.Core.Exceptions
{
    /// <summary>
    /// WebApiExceptionFilter.
    /// </summary>
    /// <seealso cref="ExceptionFilterAttribute" />
    public sealed class WebApiExceptionFilter : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiExceptionFilter"/> class.
        /// </summary>
        public WebApiExceptionFilter()
                            : base(typeof(WebApiExceptionFilterImpl))
        {
        }

        /// <summary>
        /// WebApiExceptionFilterImpl.
        /// </summary>
        /// <seealso cref="ExceptionFilterAttribute" />
        public sealed class WebApiExceptionFilterImpl : ExceptionFilterAttribute
        {
            /// <summary>
            /// The logger.
            /// </summary>
            private readonly ILogger logger;

            /// <summary>
            /// Initializes a new instance of the <see cref="WebApiExceptionFilterImpl"/> class.
            /// </summary>
            /// <param name="logger">The logger.</param>
            public WebApiExceptionFilterImpl(ILogger logger)
            {
                this.logger = logger;
            }

            /// <summary>
            /// OnException.
            /// </summary>
            /// <param name="context"></param>
            /// <inheritdoc />
            public override void OnException(ExceptionContext context)
            {
                WebAPIError apiError = null;
                if (context.Exception is WebApiException)
                {
                    // Here we handle known MyWallet API errors
                    var ex = context.Exception as WebApiException;
                    context.Exception = null;
                    context.HttpContext.Response.StatusCode = (int)ex.StatusCode;
                    logger.Write(LogEventLevel.Error, $"MyWallet API thrown error: {ex.Message}", ex);
                }
                else if (context.Exception is UnauthorizedAccessException)
                {
                    apiError = new WebAPIError("Unauthorized Access");
                    context.HttpContext.Response.StatusCode = 401;
                }
                else
                {
                    // Unhandled errors
#if !DEBUG
                        var msg = "An unhandled error occurred.";
                        string stack = null;
#else
                    var msg = context.Exception.GetBaseException().Message;
                    string stack = context.Exception.StackTrace;
#endif

                    apiError = new WebAPIError(msg)
                    {
                        detail = stack,
                    };

                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    // handle logging here
                    logger.Write(LogEventLevel.Error, context.Exception, msg);
                }

                // always return a JSON result
                context.Result = new JsonResult(apiError);

                base.OnException(context);
            }
        }
    }
}
