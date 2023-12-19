using Newtonsoft.Json;
using Platinum.Core.Extensions;
using System.Net.Http.Headers;
using System.Text;

namespace Platinum.Core.Utils.Apis
{
    /// <summary>
    /// ApiUtilities.
    /// </summary>
    public static class ApiUtils
    {
        /// <summary>
        /// Gets the microsoft date format settings.
        /// </summary>
        /// <value>
        /// The microsoft date format settings.
        /// </value>
        public static JsonSerializerSettings MicrosoftDateFormatSettings => new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
        };

        /// <summary>
        /// Creates the content of the HTTP.
        /// </summary>
        /// <typeparam name="T">Type of content</typeparam>
        /// <param name="content">The content.</param>
        /// <returns>HttpContent.</returns>
        public static HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Creates the content of the form URL encoded.
        /// </summary>
        /// <typeparam name="T">Type of content</typeparam>
        /// <param name="content">The content.</param>
        /// <returns>HttpContent.</returns>
        public static HttpContent CreateFormUrlEncodedContent<T>(T content)
        {
            var form = new FormUrlEncodedContent(content.ToDictionary());
            form.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return form;
        }
    }
}
