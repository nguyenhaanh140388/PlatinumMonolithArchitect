using Platinum.Core.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Platinum.Core.Utils.Apis;

namespace Platinum.Infrastructure.RestClients
{
    /// <summary>
    /// ApiClient.
    /// </summary>
    public class ApiClient
    {
        /// <summary>
        /// The HTTP client.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        /// <value>
        /// The base address.
        /// </value>
        private Uri baseAddress;

        /// <summary>
        /// The username.
        /// </summary>
        private string username;

        /// <summary>
        /// The password.
        /// </summary>
        private string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" /> class.
        /// </summary>
        public ApiClient()
        {
            httpClient = new HttpClient();
            AddHeaders();
            //SetBasicAuthentication();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" /> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <exception cref="ArgumentNullException">BaseAddress.</exception>
        public ApiClient(Uri baseAddress)
        {
            this.baseAddress = baseAddress ?? throw new ArgumentNullException("EAIRESTBaseAddress");
            httpClient = new HttpClient();
            AddHeaders();
            //SetBasicAuthentication();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient" /> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="ArgumentNullException">baseAddress
        /// or
        /// Username
        /// or
        /// Password.</exception>
        public ApiClient(Uri baseAddress, string userName, string password)
        {
            if (ServiceUserName == null)
            {
                throw new ArgumentNullException("Username");
            }

            if (ServicePassword == null)
            {
                throw new ArgumentNullException("Password");
            }

            EAIRESTBaseAddress = baseAddress ?? throw new ArgumentNullException("EAIRESTBaseAddress");
            ServiceUserName = userName;
            ServicePassword = password;
            httpClient = new HttpClient();
            AddHeaders();
            //SetBasicAuthentication();
        }

        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        /// <value>
        /// The base address.
        /// </value>
        public Uri EAIRESTBaseAddress
        {
            get
            {
                return baseAddress;
            }

            set
            {
                baseAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string ServiceUserName
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string ServicePassword
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<TContent> GetAsync<TContent>(string relativePath, string queryString = "")
        {
            Uri requestUrl = CreateRequestUri(relativePath, queryString);
            SetBasicAuthentication();
            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TContent>(data);
        }

        /// <summary>
        /// Common method for making POST calls.
        /// </summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<Message<TContent>> PostAsync<TContent>(TContent content, string relativePath, string queryString = "", bool formUrlEncodedContent = false)
        {
            Uri requestUrl = CreateRequestUri(relativePath, queryString);
            SetBasicAuthentication();

            HttpContent httpContent = !formUrlEncodedContent ? ApiUtils.CreateHttpContent(content) : ApiUtils.CreateFormUrlEncodedContent(content);

            var response = await httpClient.PostAsync(requestUrl.ToString(), httpContent);
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Message<TContent>>(data);
        }

        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<TOut> PostAsync<TOut, TIn>(TIn content, string relativePath, string queryString = "", bool formUrlEncodedContent = false)
        {
            Uri requestUrl = CreateRequestUri(relativePath, queryString);
            SetBasicAuthentication();

            HttpContent httpContent = !formUrlEncodedContent ? ApiUtils.CreateHttpContent(content) : ApiUtils.CreateFormUrlEncodedContent(content);

            var response = await httpClient.PostAsync(requestUrl.ToString(), httpContent);
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TOut>(data);
        }

        /// <summary>
        /// Sets the basic authentication.
        /// </summary>
        /// <exception cref="ArgumentNullException">Username
        /// or
        /// Password.</exception>
        private void SetBasicAuthentication()
        {
            if (ServiceUserName == null)
            {
                throw new ArgumentNullException("Username");
            }

            if (ServicePassword == null)
            {
                throw new ArgumentNullException("Password");
            }

            var byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", ServiceUserName, ServicePassword));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        /// <summary>
        /// Creates the request URI.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">baseAddress.</exception>
        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            if (baseAddress == null)
            {
                throw new ArgumentNullException("EAIRESTBaseAddress");
            }

            var endpoint = new Uri(baseAddress, relativePath);
            var uriBuilder = new UriBuilder(endpoint)
            {
                Query = queryString,
            };

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Adds the headers.
        /// </summary>
        private void AddHeaders()
        {
            httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        }
    }
}
