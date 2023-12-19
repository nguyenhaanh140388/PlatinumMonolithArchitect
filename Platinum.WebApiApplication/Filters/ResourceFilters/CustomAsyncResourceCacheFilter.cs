// <copyright file="CustomAsyncResourceCacheFilter.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using ILogger = Serilog.ILogger;

namespace Anhny010920WebAppApi.Filters.ResourceFilters
{
    /// <summary>
    /// LoggingActionFilter.
    /// </summary>
    /// <seealso cref="TypeFilterAttribute" />
    public sealed class CustomAsyncResourceCacheFilter : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAsyncResourceCacheFilter" /> class.
        /// </summary>
        public CustomAsyncResourceCacheFilter()
                            : base(typeof(CustomAsyncResourceCacheFilterImpl))
        {
        }

        /// <summary>
        /// CustomAsyncResourceCacheFilter.
        /// </summary>
        /// <seealso cref="IAsyncResourceFilter" />
        /// <seealso cref="IResourceFilter" />
        public class CustomAsyncResourceCacheFilterImpl : IAsyncResourceFilter
        {
            /// <summary>
            /// The configuration.
            /// </summary>
            private readonly IConfiguration configuration;

            /// <summary>
            /// The cache.
            /// </summary>
            private readonly IMemoryCache cache;

            /// <summary>
            /// The logger.
            /// </summary>
            private readonly ILogger logger;

            /// <summary>
            /// The cache duration.
            /// </summary>
            private int cacheDuration = 0;

            /// <summary>
            /// The cache enabled.
            /// </summary>
            private bool cacheEnabled = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomAsyncResourceCacheFilterImpl"/> class.
            /// </summary>
            /// <param name="configuration">The configuration.</param>
            /// <param name="cache">The cache.</param>
            /// <param name="logger">The logger.</param>
            public CustomAsyncResourceCacheFilterImpl(IConfiguration configuration, IMemoryCache cache, ILogger logger)
            {
                this.configuration = configuration;
                this.cache = cache;
                this.logger = logger;
                InitFilter();
            }

            ///// <summary>
            ///// Called when [resource executed].
            ///// </summary>
            ///// <param name="context">The context.</param>
            // public void OnResourceExecuted(ResourceExecutedContext context)
            // {
            //    if (this.cacheEnabled)
            //    {
            //        string cachekey = string.Join(":", new string[] { context.RouteData.Values["controller"].ToString(), context.RouteData.Values["action"].ToString() });
            //        if (this.cache.TryGetValue(cachekey, out string cachedContent))
            //        {
            //            if (!string.IsNullOrEmpty(cachedContent))
            //            {
            //                context.Result = new ContentResult()
            //                {
            //                    Content = cachedContent,
            //                };
            //            }
            //        }
            //    }
            // }

            ///// <summary>
            ///// Called when [resource executing].
            ///// </summary>
            ///// <param name="context">The context.</param>
            // public void OnResourceExecuting(ResourceExecutingContext context)
            // {
            //    try
            //    {
            //        if (this.cacheEnabled)
            //        {
            //            if (this.cache != null)
            //            {
            //                string cachekey = string.Join(":", new string[] { context.RouteData.Values["controller"].ToString(), context.RouteData.Values["action"].ToString() });
            //                if (context.Result is ContentResult result)
            //                {
            //                    string cachedContent = string.Empty;
            //                    this.cache.Set(cachekey, result.Content, DateTime.Now.AddSeconds(this.cacheDuration));
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        this.logger.Error("Error caching in MyResourceFilter", ex);
            //    }
            // }

            /// <summary>
            /// Called asynchronously before the rest of the pipeline.
            /// </summary>
            /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ResourceExecutingContext" />.</param>
            /// <param name="next">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ResourceExecutionDelegate" />. Invoked to execute the next resource filter or the remainder
            /// of the pipeline.</param>
            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                // code executed before the filter executes
                try
                {
                    if (cacheEnabled)
                    {
                        if (cache != null)
                        {
                            string cachekey = string.Join(":", new string[] { context.RouteData.Values["controller"].ToString(), context.RouteData.Values["action"].ToString() });
                            if (context.Result is ObjectResult result)
                            {
                                string cachedContent = string.Empty;
                                cache.Set(cachekey, result.Value, DateTime.Now.AddSeconds(cacheDuration));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error caching in MyResourceFilter", ex);
                }

                await next();

                // code executed after the filter executes
                if (cacheEnabled)
                {
                    string cachekey = string.Join(":", new string[] { context.RouteData.Values["controller"].ToString(), context.RouteData.Values["action"].ToString() });
                    if (cache.TryGetValue(cachekey, out string cachedContent))
                    {
                        if (!string.IsNullOrEmpty(cachedContent))
                        {
                            context.Result = new ContentResult()
                            {
                                Content = cachedContent,
                            };
                        }
                    }
                }
            }

            /// <summary>
            /// Initializes the filter.
            /// </summary>
            private void InitFilter()
            {
                if (!bool.TryParse(
                configuration.GetSection("AppSettings:CacheEnabled").Value,
                out cacheEnabled))
                {
                    cacheEnabled = false;
                }

                if (!int.TryParse(
                configuration.GetSection("AppSettings:CacheDuration").Value,
                out cacheDuration))
                {
                    cacheDuration = 21600; // 6 hours cache by default
                }
            }
        }
    }
}
