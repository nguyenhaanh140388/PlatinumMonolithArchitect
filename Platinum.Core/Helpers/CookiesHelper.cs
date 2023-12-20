// <copyright file="Cookies.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Core.Helpers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;
    using Platinum.Core.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Cookies.
    /// </summary>
    public static class CookiesHelper
    {
        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expireTime">The expire time.</param>
        public static void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions
            {
                IsEssential = true,
                HttpOnly = false,
                Secure = false,
                Path = "/",
            };

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            }
            else
            {
                option.Expires = DateTime.Now.AddMilliseconds(10);
            }

            ApplicationHttpContext.HttpContext.Response.Cookies.Append(key, value, option);
        }

        /// <summary>
        /// Extracts the cookies from response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>IDictionary.<string, string></returns>
        public static IDictionary<string, string> ExtractCookiesFromResponse(this HttpResponse response)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            if (response.Headers.TryGetValue("Set-Cookie", out StringValues values))
            {
                SetCookieHeaderValue.ParseList(values.ToList()).ToList().ForEach(cookie =>
                {
                    result.Add(
                        key: cookie.Name.Value,
                        value: cookie.Value.Value);
                });
            }

            return result;
        }
    }
}
